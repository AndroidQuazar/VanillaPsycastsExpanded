namespace VanillaPsycastsExpanded.Conflagrator
{
    using RimWorld;
    using Verse;

    [StaticConstructorOnStartup]
    public class FireBeam : PowerBeam
    {
        private static readonly ThingDef Mote_FireBeam = ThingDef.Named("VPE_Mote_FireBeam");

        public override void StartStrike()
        {
            base.StartStrike();
            Mote mote = this.Position.GetFirstThing<Mote>(this.Map);
            mote.Destroy();
            mote               = (Mote) ThingMaker.MakeThing(Mote_FireBeam);
            mote.exactPosition = this.Position.ToVector3Shifted();
            mote.Scale         = 90f;
            mote.rotationRate  = 1.2f;
            GenSpawn.Spawn(mote, this.Position, this.Map);
        }
    }
}