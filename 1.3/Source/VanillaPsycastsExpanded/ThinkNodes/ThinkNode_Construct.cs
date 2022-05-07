using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	public class ThinkNode_Construct : ThinkNode_Conditional
	{


		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.def == VPE_DefOf.VPE_RockConstruct || pawn.def == VPE_DefOf.VPE_SteelConstruct)
			{
				return true;
			}
			return false;
		}
	}
}
