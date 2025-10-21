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
            ProjectileProperties projectileProps = projectile?.projectile;
            ThingDef equipmentDef = EquipmentSource?.def;
            if (equipmentDef is null)
            {
                Log.Error($"Unable to retrieve weapon def from <color=teal>{GetType()}</color>. Please report to Oskar or Smash Phil.");
                return true;
            }

            MoteProperties moteProps = equipmentDef.GetModExtension<MoteProperties>();
            if (moteProps is null)
            {
                // Warn-once and proceed without throwing motes to avoid error spam
                Log.WarningOnce($"<color=teal>{GetType()}</color> cannot be used without <color=teal>MoteProperties</color> DefModExtension on {equipmentDef.defName}. Motes will not be thrown.", Gen.HashCombine(equipmentDef.defName.GetHashCode(), "MoteProperties".GetHashCode()));
                return true;
            }

            // Compute size safely; default to a small value if projectile props are missing
            int damage = 0;
            if (projectileProps != null)
            {
                try
                {
                    damage = projectileProps.GetDamageAmount(caster);
                }
                catch
                {
                    damage = 0;
                }
            }
            float size = moteProps.Size(damage);

            for (int i = 0; i < moteProps.numTimesThrown; i++)
            {
                float relAngle = Quaternion.LookRotation(CurrentTarget.CenterVector3 - Caster.Position.ToVector3Shifted()).eulerAngles.y;
                Vector3 origin = caster.PositionHeld.ToVector3Shifted();
                Map map = caster.MapHeld;
                if (moteProps.moteDef != null)
                    SmokeMaker.ThrowMoteDef(moteProps.moteDef, origin, map, size, moteProps.Velocity, relAngle + moteProps.Angle, moteProps.Rotation);
                if (moteProps.fleckDef != null)
                    SmokeMaker.ThrowFleckDef(moteProps.fleckDef, origin, map, size, moteProps.Velocity, relAngle + moteProps.Angle, moteProps.Rotation);
            }

            return true;

        }
    }
}
