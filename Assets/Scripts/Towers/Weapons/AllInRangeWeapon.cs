using PSG.BattlefieldAndGuns.Core;

namespace PSG.BattlefieldAndGuns.Towers
{
    public class AllInRangeWeapon : Weapon
    {
        /// <summary>
        /// Does the tower have valid targets?
        /// </summary>
        /// <returns></returns>
        public override bool HasValidTargets()
        {
            targets = targeter.GetTargets();
            return targets.Length > 0;
        }

        /// <summary>
        /// Shoot the weapon.
        /// </summary>
        public override void Fire()
        {
            if (targets.Length > 0)
            {
                base.Fire();
                foreach(var target in targets)
                {
                    target.DealDamage(Damage);
                }
            }
        }
    }
}
