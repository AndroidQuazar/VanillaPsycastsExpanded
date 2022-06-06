using Verse;

namespace VanillaPsycastsExpanded
{
    public class Hediff_BodyConsumption : Hediff_Liferot
    {
        public Pawn consumer;
        public override void Notify_PawnKilled()
        {
            base.Notify_PawnKilled();
            if (consumer.Spawned && consumer.Map == this.pawn.Map && !consumer.Dead)
            {
                MoteBetween mote = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_SoulOrbTransfer);
                mote.Attach(this.pawn, this.consumer);
                mote.exactPosition = this.pawn.DrawPos;
                GenSpawn.Spawn(mote, this.pawn.Position, consumer.Map);
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref consumer, "consumer");
        }
    }
}