namespace VanillaPsycastsExpanded.Chronopath
{
    using Verse;
    using VFECore.Abilities;

    public class Ability_Meditate : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            this.pawn.psychicEntropy.OffsetPsyfocusDirectly(1f - this.pawn.psychicEntropy.CurrentPsyfocus);
            this.pawn.Psycasts().SpentPoints(-300);
        }
    }
}