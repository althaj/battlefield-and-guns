using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSG.BattlefieldAndGuns.Core;
using System.Linq;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class SingleTargeter : Targeter
    {
        public override Enemy[] GetTargets()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>().OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();

            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemies[i].transform.position) <= Range)
                    return new Enemy[] { enemies[i] };
            }
            return new Enemy[0];
        }
    }
}
