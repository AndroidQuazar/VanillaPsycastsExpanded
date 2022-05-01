namespace VanillaPsycastsExpanded.Chronopath
{
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_AdvanceSeason : Ability
    {
        private int ticksAdvanceLeft;

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            this.ticksAdvanceLeft = Mathf.CeilToInt(GenDate.HoursPerDay * 15f);
        }

        public override void Tick()
        {
            base.Tick();
            if (this.ticksAdvanceLeft > 0)
            {
                this.ticksAdvanceLeft--;
                Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + GenDate.TicksPerHour);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.ticksAdvanceLeft, nameof(this.ticksAdvanceLeft));
        }
    }
}