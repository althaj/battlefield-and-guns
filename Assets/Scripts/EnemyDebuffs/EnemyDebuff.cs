using PSG.BattlefieldAndGuns.Core;

namespace PSG.BattlefieldAndGuns.EnemyDebuffs
{
	public abstract class EnemyDebuff
	{
		public float DurationLeft { get; set; }

		public abstract void Register(Enemy enemy);

		public abstract void Unregister(Enemy enemy);

		/// <summary>
		/// Ticks the debuff. Override if you need to appply some effect every frame.
		/// </summary>
		/// <param name="time">Time elapsed (Time.deltaTime)</param>
		/// <returns>True if the buff should ended.</returns>
		public bool Tick(float time)
		{
			DurationLeft -= time;

			return DurationLeft <= 0;
		}
	}
}
