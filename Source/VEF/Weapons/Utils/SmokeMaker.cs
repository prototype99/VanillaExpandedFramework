using UnityEngine;
using Verse;

namespace VEF.Weapons
{
    public static class SmokeMaker
    {
        public static void ThrowDef(Def effectDef, Vector3 loc, Map map, float size, float velocity, float angle, float rotation)
        {
            size = Rand.Range(1f, 2f) * size;

            switch (effectDef)
            {
                case ThingDef moteDef:
                    if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority) return;
                    MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef);
                    moteThrown.Scale = size;
                    moteThrown.rotationRate = rotation;
                    moteThrown.exactPosition = loc;
                    moteThrown.SetVelocity(angle, velocity);
                    GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
                    break;
                case FleckDef fleckDef:
                    FleckCreationData data = default;
                    data.def = fleckDef;
                    data.spawnPosition = loc;
                    data.scale = size;
                    data.rotationRate = rotation;
                    data.velocitySpeed = velocity;
                    data.velocityAngle = angle;
                    map.flecks.CreateFleck(data);
                    break;
            }
        }

        public static void ThrowSmokeTrail(Vector3 loc, float size, Map map, string defName)
        {
            if (GenView.ShouldSpawnMotesAt(loc, map) && !map.moteCounter.Saturated)
            {
                MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDef.Named(defName), null);
                moteThrown.Scale = Rand.Range(2f, 3f) * size;
                moteThrown.exactPosition = loc;
                moteThrown.rotationRate = Rand.Range(-0.5f, 0.5f);
                moteThrown.SetVelocity((float)Rand.Range(30, 40), Rand.Range(0.008f, 0.012f));
                GenSpawn.Spawn(moteThrown, IntVec3Utility.ToIntVec3(loc), map, WipeMode.Vanish);
            }
        }

        public static void ThrowFlintLockSmoke(Vector3 loc, Map map, float size)
        {
            if (!GenView.ShouldSpawnMotesAt(loc, map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }
            MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(InternalDefOf.VEF_FlintlockSmoke, null);
            moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
            moteThrown.rotationRate = Rand.Range(-30f, 30f);
            moteThrown.exactPosition = loc;
            moteThrown.SetVelocity((float)Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
            GenSpawn.Spawn(moteThrown, IntVec3Utility.ToIntVec3(loc), map, WipeMode.Vanish);
        }
    }
}
