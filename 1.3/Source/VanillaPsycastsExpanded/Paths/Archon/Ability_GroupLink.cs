namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using UnityEngine;
    using Verse;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_GroupLink : Ability
    {
        public override Hediff ApplyHediff(Pawn targetPawn)
        {
            var hediff = base.ApplyHediff(targetPawn) as Hediff_GroupLink;
            hediff.LinkAllPawnsAround();
            return hediff;
        }
    }
}