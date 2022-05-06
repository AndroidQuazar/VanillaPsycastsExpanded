namespace VanillaPsycastsExpanded
{
    using HarmonyLib;
    using RimWorld;
    using System.Diagnostics;
    using UnityEngine;
    using Verse;

    [HarmonyPatch(typeof(Transferable), "AdjustBy")]
	public static class Transferable_AdjustBy_Patch
    {
		public static void Postfix(Transferable __instance)
		{
            if (Find.WindowStack.IsOpen<Dialog_Trade>() && TradeSession.trader != null && __instance.ThingDef == VPE_DefOf.VPE_Eltex 
                &&  TradeSession.trader.Faction != Faction.OfEmpire && __instance.CountToTransferToDestination > 0)
            {
                if (TradeSession.giftMode)
                {
                    Messages.Message("VPE.GiftingEltexWarning".Translate(), MessageTypeDefOf.CautionInput);
                }
                else
                {
                    Messages.Message("VPE.SellingEltexWarning".Translate(), MessageTypeDefOf.CautionInput);
                }
            }
		}
    }
}