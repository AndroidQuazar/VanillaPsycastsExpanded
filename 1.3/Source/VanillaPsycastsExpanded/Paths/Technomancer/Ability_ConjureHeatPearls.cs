namespace VanillaPsycastsExpanded.Technomancer
{
    using System.Collections.Generic;
    using HarmonyLib;
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_ConjureHeatPearls : Ability
    {
        private static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> currentEntropy =
            AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>("currentEntropy");

        public override bool IsEnabledForPawn(out string reason)
        {
            if (!base.IsEnabledForPawn(out reason)) return false;

            if (this.pawn.psychicEntropy.EntropyValue - this.pawn.GetStatValue(VPE_DefOf.VPE_PsychicEntropyMinimum) >= 20f) return true;

            reason = "VPE.NotEnoughHeat".Translate();
            return false;
        }

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            currentEntropy(this.pawn.psychicEntropy) -= 20f;
            Thing   pearl = ThingMaker.MakeThing(VPE_DefOf.VPE_HeatPearls);
            IntVec3 cell  = this.pawn.Position + GenRadial.RadialPattern[Rand.RangeInclusive(2, GenRadial.NumCellsInRadius(4.9f))];
            GenExplosion.DoExplosion(cell, this.pawn.Map, 1.9f, DamageDefOf.Bomb, this.pawn, ignoredThings: new List<Thing> {this.pawn, pearl});
            GenSpawn.Spawn(pearl, cell, this.pawn.Map);
        }
    }
}