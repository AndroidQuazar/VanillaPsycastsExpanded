// Verse.AI.MentalState_Manhunter
using RimWorld;
using Verse;
using Verse.AI;
namespace VanillaPsycastsExpanded
{
	public class MentalState_FriendlyManhunter : MentalState_Manhunter
	{
		public override bool ForceHostileTo(Thing t)
		{
			
			if (t.Faction != null && t.Faction != Faction.OfPlayer)
			{
				return ForceHostileTo(t.Faction);
			}
			return false;
		}

		public override bool ForceHostileTo(Faction f)
		{
			if (!f.def.humanlikeFaction)
			{
				return f == Faction.OfMechanoids;
			}
			return true;
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
