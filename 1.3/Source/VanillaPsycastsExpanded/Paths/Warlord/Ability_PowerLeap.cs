namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_PowerLeap : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            var map = Caster.Map;
            var flyer = (JumpingPawn)PawnFlyer.MakeFlyer(VPE_DefOf.VPE_JumpingPawn, CasterPawn, target.Cell);
            flyer.ability = this;
            flyer.target = target.CenterVector3;
            GenSpawn.Spawn(flyer, Caster.Position, map);
            base.Cast(target);
        }
    }
}