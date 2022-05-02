namespace VanillaPsycastsExpanded
{
    using RimWorld;
using System.Collections.Generic;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_AggressiveHeatDump : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            var explosionRadius = Mathf.Min(this.pawn.psychicEntropy.EntropyValue / 20f, 9f);
            this.pawn.psychicEntropy.RemoveAllEntropy();
            MakeStaticFleck(target.Cell, target.Pawn.Map, FleckDefOf.PsycastAreaEffect, explosionRadius, 0);
            MakeStaticFleck(target.Cell, target.Pawn.Map, VPE_DefOf.VPE_AggresiveHeatDump, explosionRadius, 0);
            GenExplosion.DoExplosion(target.Cell, pawn.Map, explosionRadius, DamageDefOf.Flame, pawn, ignoredThings: new List<Thing> { pawn });
        }
    }
}