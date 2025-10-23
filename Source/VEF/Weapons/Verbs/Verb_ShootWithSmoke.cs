using UnityEngine;

using Verse;

namespace VEF.Weapons
{
    public class Verb_ShootWithSmoke : Verb_Shoot
    {
        protected override bool TryCastShot()
        {
            if (!base.TryCastShot()) return false;
            ThingDef projectile = this.GetProjectile();
            ThingDef equipmentDef = EquipmentSource?.def;
            if (equipmentDef is null)
            {
                Log.Error($"Unable to retrieve weapon def from <color=teal>{GetType()}</color>. Please report to Oskar or Smash Phil.");
                return true;
            }

            MoteProperties moteProps = equipmentDef.GetModExtension<MoteProperties>();
            if (moteProps is null)
            {
                Log.ErrorOnce($"<color=teal>{GetType()}</color> cannot be used without <color=teal>MoteProperties</color> DefModExtension. Motes will not be thrown.", Gen.HashCombine(projectile.GetHashCode(), "MoteProperties".GetHashCode()));
                return true;
            }

            for (int i = 0; i < moteProps.numTimesThrown; i++)
            {
                ThrowEffect(moteProps.moteDef, projectile, moteProps);
                ThrowEffect(moteProps.fleckDef, projectile, moteProps);
            }

            return true;

        }

        private void ThrowEffect(Def effectDef, ThingDef projectile, MoteProperties moteProps)
        {
            if (effectDef != null)
            {
                SmokeMaker.ThrowDef(
                    effectDef,
                    // origin
                    caster.PositionHeld.ToVector3Shifted(),
                    // map
                    caster.MapHeld,
                    // size
                    moteProps.Size(projectile.projectile.GetDamageAmount(caster)),
                    moteProps.Velocity,
                    // relangle
                    Quaternion.LookRotation(CurrentTarget.CenterVector3 - Caster.Position.ToVector3Shifted()).eulerAngles.y + moteProps.Angle,
                    moteProps.Rotation
                    );
            }
        }
    }
}
