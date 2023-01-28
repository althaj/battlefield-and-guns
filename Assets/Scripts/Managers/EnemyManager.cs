using PSG.BattlefieldAndGuns.Core;
using PSG.BattlefieldAndGuns.UI;
using PSG.BattlefieldAndGuns.Utility;
using PSG.RNG;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace PSG.BattlefieldAndGuns.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public enum WaveType
        {
            [Description("Randomized enemy units")]
            Random = 0,
            [Description("Heavy enemy units")]
            Strong = 1
        }

        #region serialized variables
        [SerializeField]
        private GameObject[] enemyPrefabs;
        [SerializeField]
        private int initialWaveStrength;
        [SerializeField]
        private float waveStrengthScaling;
        [SerializeField]
        private Transform spawnTranform;
        [SerializeField]
        private float timeBeforeNextWave = 60;
        #endregion

        #region private variables
        private int currentWave = 0;
        private int currentWaveStrength;
        private List<Enemy> spawnedEnemies;
        private float currentTimeBeforeNextWave = 0;
        private bool waitingForWave;

        private TowerManager towerManager;
        private BuffManager buffManager;
        private GameUI gameUI;

        // buffs
        private float healthMultiplier;
        private float speedMultiplier;
        private float rewardMultiplier;
        private float waveStrengthMultiplier;
        #endregion

        #region properties
        public float CurrentTimeBeforeNextWave { get => currentTimeBeforeNextWave; }

        public Vector3? SpawnPoint { get => spawnTranform != null ? spawnTranform.position : null;  }

        public Vector3[] Path { get; set; }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            spawnedEnemies = new List<Enemy>();
            towerManager = FindObjectOfType<TowerManager>();
            buffManager = FindObjectOfType<BuffManager>();
            gameUI = FindObjectOfType<GameUI>();
            StartWaveCountdown();
        }

        // Update is called once per frame
        void Update()
        {
            if (waitingForWave)
            {
                if (currentTimeBeforeNextWave > 0)
                    currentTimeBeforeNextWave -= Time.deltaTime;
                else
                    NextWave();
            }
        }

        /// <summary>
        /// Start the waiting for the next wave.
        /// </summary>
        public void StartWaveCountdown()
        {
            currentTimeBeforeNextWave = timeBeforeNextWave;
            waitingForWave = true;
        }

        /// <summary>
        /// Proceed to next wave.
        /// </summary>
        private void NextWave()
        {
            waitingForWave = false;
            currentWave++;
            currentWaveStrength = initialWaveStrength * (int)Mathf.Pow(waveStrengthScaling, currentWave);
            StartCoroutine(SpawnEnemies());
        }

        /// <summary>
        /// Spawn enemies of the current wave.
        /// </summary>
        private IEnumerator SpawnEnemies()
        {
            Dictionary<Enemy, GameObject> enemyComponents = new Dictionary<Enemy, GameObject>();

            healthMultiplier = buffManager.GetAppliedMultiplier(BuffData.BuffType.EnemyHealth);
            speedMultiplier = buffManager.GetAppliedMultiplier(BuffData.BuffType.EnemySpeed);
            rewardMultiplier = buffManager.GetAppliedMultiplier(BuffData.BuffType.EnemyReward);
            waveStrengthMultiplier = buffManager.GetAppliedMultiplier(BuffData.BuffType.WaveStrength);

            int remainingStrength = (int)(currentWaveStrength * waveStrengthMultiplier);

            WaveType waveType = RNGManager.Manager[Constants.ENEMY_MANAGER_RNG_TITLE].NextEnumValue<WaveType>();

            gameUI.ShowMessage($"Wave {currentWave}", waveType.GetDescription(), MessageDisplayDuration.Short);

            switch (waveType)
            {
                case WaveType.Random:
                    break;
                case WaveType.Strong:
                    enemyPrefabs = enemyPrefabs.OrderByDescending(x => x.GetComponent<Enemy>().Strength).ToArray();
                    enemyComponents = enemyPrefabs.ToDictionary(x => x.GetComponent<Enemy>());
                    break;
                default:
                    enemyComponents = enemyPrefabs.ToDictionary(x => x.GetComponent<Enemy>());
                    break;
            }

            int spawnedUnits = 0;

            while (remainingStrength > 0)
            {
                if(waveType == WaveType.Random)
                {
                    enemyComponents = enemyPrefabs.OrderBy(x => RNGManager.Manager[Constants.ENEMY_MANAGER_RNG_TITLE].NextFloat()).ToDictionary(x => x.GetComponent<Enemy>());
                }

                Enemy selectedEnemy = enemyComponents.Where(x => x.Key.Strength <= remainingStrength).FirstOrDefault().Key;
                GameObject enemyPrefab = enemyComponents[selectedEnemy];
                SpawnEnemy(enemyPrefab);
                remainingStrength -= selectedEnemy.Strength;

                if (spawnedEnemies.Count % 25 == 0)
                    yield return new WaitForSeconds(6);
            }
        }

        /// <summary>
        /// Spawn an enemy from a prefab.
        /// </summary>
        /// <param name="prefab">Enemy prefab.</param>
        private void SpawnEnemy(GameObject prefab)
        {
            Vector3 randomOffset = new Vector3(
                    RNGManager.Manager[Constants.ENEMY_MANAGER_RNG_TITLE].NextFloat(-1f, 1f),
                    0,
                    RNGManager.Manager[Constants.ENEMY_MANAGER_RNG_TITLE].NextFloat(-1f, 1f)
                );

            GameObject go = Instantiate(prefab, spawnTranform.position + randomOffset, spawnTranform.rotation);
            Enemy spawnedEnemy = go.GetComponent<Enemy>();
            spawnedEnemy.EnemyManager = this;
            spawnedEnemy.Path = Path;
            spawnedEnemy.ApplyBuffs(healthMultiplier, speedMultiplier, rewardMultiplier);
            spawnedEnemies.Add(spawnedEnemy);
        }

        /// <summary>
        /// Kill an enemy, removing it from the list of spawned enemies.
        /// </summary>
        /// <param name="enemy">Enemy to remove.</param>
        public void KillEnemy(Enemy enemy)
        {
            towerManager.AddReward(enemy.Reward);
            RemoveEnemy(enemy);
        }

        /// <summary>
        /// Removes the enemy from the level.
        /// </summary>
        /// <param name="enemy">Enemy to remove.</param>
        public void RemoveEnemy(Enemy enemy)
        {
            spawnedEnemies.Remove(enemy);

            if (!waitingForWave && spawnedEnemies.Count == 0)
            {
                if (currentWave % 5 == 0)
                {
                    gameUI.ShowBuffPanel();
                }
                else
                {
                    buffManager.ApplyRandomDebuff();
                    StartWaveCountdown();
                }
            }

            Destroy(enemy.gameObject);
        }

        /// <summary>
        /// Get the closest enemy to a position.
        /// </summary>
        /// <param name="position">Position to compare the distance with.</param>
        /// <param name="range">Maximum distance between the enemy and the object.</param>
        /// <returns></returns>
        public Enemy GetClosestEnemy(Vector3 position, float range)
        {
            if (spawnedEnemies == null || spawnedEnemies.Count == 0)
                return null;

            // Order the enemies by distance and get the first one if it's within the range.
            IOrderedEnumerable<Enemy> orderedEnemies = spawnedEnemies.OrderBy(x => Vector3.Distance(position, x.transform.position));
            if (Vector3.Distance(position, orderedEnemies.First().transform.position) <= range)
                return orderedEnemies.First();
            else
                return null;
        }

        public IEnumerable<Enemy> GetEnemiesInRange(Vector3 position, float range)
        {
            return spawnedEnemies.Where(x => Vector3.Distance(position, x.transform.position) <= range);
        }
    } 
}
