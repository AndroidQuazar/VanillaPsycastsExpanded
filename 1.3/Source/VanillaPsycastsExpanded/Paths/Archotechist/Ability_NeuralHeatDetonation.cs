namespace VanillaPsycastsExpanded
{
    using RimWorld;
using System.Collections.Generic;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_NeuralHeatDetonation : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            var explosionRadius = (this.pawn.psychicEntropy.EntropyValue / 10f) * this.pawn.GetStatValue(StatDefOf.PsychicSensitivity);
            this.pawn.psychicEntropy.RemoveAllEntropy();
            MakeStaticFleck(target.Cell, target.Pawn.Map, FleckDefOf.PsycastAreaEffect, explosionRadius, 0);
            GenExplosion.DoExplosion(target.Cell, pawn.Map, explosionRadius, DamageDefOf.Flame, pawn, ignoredThings: new List<Thing> { pawn });
        }
    }
}