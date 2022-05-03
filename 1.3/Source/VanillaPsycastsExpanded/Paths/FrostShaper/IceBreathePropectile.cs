namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using System.Collections.Generic;
    using Verse;
    using Verse.AI;
    using Verse.Sound;
	using VFECore;
	public class IceBreathePropectile : ExpandableProjectile
	{
		public override void DoDamage(IntVec3 pos)
		{
			base.DoDamage(pos);
			try
			{
				if (pos != this.launcher.Position && this.launcher.Map != null && GenGrid.InBounds(pos, this.launcher.Map))
				{
					Map.snowGrid.AddDepth(pos, 0.5f);
					var list = this.launcher.Map.thingGrid.ThingsListAt(pos);
					for (int num = list.Count - 1; num >= 0; num--)
					{
						if (IsDamagable(list[num]))
						{
							this.customImpact = true;
							base.Impact(list[num]);
							this.customImpact = false;
							if (list[num] is Pawn pawn)
							{
								var severityImpact = (0.5f / pawn.Position.DistanceTo(launcher.Position));
								HealthUtility.AdjustSeverity(pawn, HediffDefOf.Hypothermia, severityImpact);
								HealthUtility.AdjustSeverity(pawn, VPE_DefOf.VFEP_HypothermicSlowdown, severityImpact);
							}
						}
					}
				}
			}
			catch { };
		}

		public override bool IsDamagable(Thing t)
		{
			return t is Pawn && base.IsDamagable(t) || t.def == ThingDefOf.Fire;
		}
	}
}