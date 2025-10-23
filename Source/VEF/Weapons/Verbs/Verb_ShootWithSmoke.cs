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
            ProjectileProperties projectile2 = projectile.projectile;
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

            float size = moteProps.Size(projectile2.GetDamageAmount(caster));
            for (int i = 0; i < moteProps.numTimesThrown; i++)
            {
                float relAngle = Quaternion.LookRotation(CurrentTarget.CenterVector3 - Caster.Position.ToVector3Shifted()).eulerAngles.y;
                Vector3 origin = caster.PositionHeld.ToVector3Shifted();
                Map map = caster.MapHeld;
                ThrowEffect(moteProps.moteDef, size, relAngle, origin, map, moteProps);
                ThrowEffect(moteProps.fleckDef, size, relAngle, origin, map, moteProps);
            }

            return true;

        }

        private static void ThrowEffect(Def effectDef, float size, float relAngle, Vector3 origin, Map map, MoteProperties moteProps)
        {
            if (effectDef != null)
                SmokeMaker.ThrowDef(effectDef, origin, map, size, moteProps.Velocity, relAngle + moteProps.Angle, moteProps.Rotation);
        }
    }
}
