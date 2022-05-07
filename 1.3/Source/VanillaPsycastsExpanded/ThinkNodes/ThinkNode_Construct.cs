namespace VanillaPsycastsExpanded
{
    using Verse;
    using Verse.AI;

    public class ThinkNode_Construct : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.def == VPE_DefOf.VPE_Race_RockConstruct || pawn.def == VPE_DefOf.VPE_Race_SteelConstruct) return true;
            return false;
        }
    }
}