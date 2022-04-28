namespace VanillaPsycastsExpanded.Harmonist
{
    using RimWorld;
    using Verse;
    using Verse.AI.Group;

    public class HediffComp_MindControl : HediffComp
    {
        private Faction oldFaction;
        private Lord    oldLord;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            this.oldFaction = this.Pawn.Faction;
            this.oldLord    = this.Pawn.GetLord();
            this.Pawn.SetFaction(Faction.OfPlayer);
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            this.Pawn.SetFaction(this.oldFaction);
            this.oldLord?.AddPawn(this.Pawn);
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref this.oldFaction, nameof(this.oldFaction));
            Scribe_References.Look(ref this.oldLord,    nameof(this.oldLord));
        }
    }
}