namespace VanillaPsycastsExpanded.Harmonist
{
    using System.Linq;
    using RimWorld.Planet;
    using Verse;
    using VFECore.Abilities;

    public class Ability_HealthSwap : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            if (targets[0].Thing is not Pawn source || targets[1].Thing is not Pawn dest) return;
            MoteBetween mote = (MoteBetween) ThingMaker.MakeThing(VPE_DefOf.VPE_PsycastPsychicEffectTransfer);
            mote.Attach(source, dest);
            mote.Scale         = 1f;
            mote.exactPosition = source.DrawPos;
            GenSpawn.Spawn(mote, source.Position, source.MapHeld);

            foreach (Hediff hediff in source.health.hediffSet.hediffs.Where(this.ShouldTransfer).ToList())
            {
                source.health.RemoveHediff(hediff);
                dest.health.AddHediff(hediff, hediff.Part);
            }
        }

        private bool ShouldTransfer(Hediff hediff) => hediff is Hediff_Injury || hediff.def.tendable || hediff.def.makesSickThought;
    }
}