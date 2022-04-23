namespace VanillaPsycastsExpanded.Conflagrator
{
    using Verse;
    using VFECore.Abilities;

    [StaticConstructorOnStartup]
    public class Ability_FireTornado : Ability
    {
        private static readonly ThingDef FireTornadoDef = ThingDef.Named("VPE_FireTornado");

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            FireTornado tornado = (FireTornado) GenSpawn.Spawn(FireTornadoDef, target.Cell, this.pawn.Map);
            tornado.ticksLeftToDisappear = this.GetDurationForPawn();
        }
    }
}