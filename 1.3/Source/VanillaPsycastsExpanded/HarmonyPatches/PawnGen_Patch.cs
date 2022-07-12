using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
    using HarmonyLib;
    using RimWorld;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    using AbilityDef = VFECore.Abilities.AbilityDef;

    [HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
    [HarmonyAfter("OskarPotocki.VFECore")]
    public class PawnGen_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __result)
        {
            PawnKindAbilityExtension_Psycasts psycastExtension = __result.kindDef.GetModExtension<PawnKindAbilityExtension_Psycasts>();
            if (psycastExtension == null)
                return;


            CompAbilities comp = __result.GetComp<CompAbilities>();

            if (psycastExtension.implantDef != null)
            {
                Hediff_Psylink psylink = (__result.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier) as Hediff_Psylink);

                if (psylink == null)
                {
                    psylink = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, __result, __result.health.hediffSet.GetBrain()) as Hediff_Psylink;
                    __result.health.AddHediff(psylink);
                }


                Hediff_PsycastAbilities implant = (__result.health.hediffSet.GetFirstHediffOfDef(psycastExtension.implantDef) as Hediff_PsycastAbilities)!;

                if(implant.psylink == null)
                    implant.InitializeFromPsylink(psylink);

                foreach (PathUnlockData unlockedPath in psycastExtension.unlockedPaths)
                {
                    if (unlockedPath.path.CanPawnUnlock(__result))
                    {
                        implant.UnlockPath(unlockedPath.path);

                        int abilityCount = unlockedPath.unlockedAbilityCount.RandomInRange;

                        IEnumerable<AbilityDef> abilitySelection = new List<AbilityDef>();

                        for (int level = unlockedPath.unlockedAbilityLevelRange.min; level < unlockedPath.unlockedAbilityLevelRange.max && level < unlockedPath.path.MaxLevel; level++)
                            abilitySelection = abilitySelection.Concat(unlockedPath.path.abilityLevelsInOrder[level]);

                        List<AbilityDef> abilitySelectionList = abilitySelection.ToList();

                        while (abilitySelectionList.Any() && abilityCount > 0)
                        {
                            abilityCount--;
                            AbilityDef abilityDef = abilitySelectionList.RandomElement();

                            abilityDef.GetModExtension<AbilityExtension_Psycast>().UnlockWithPrereqs(comp);
                            implant.ChangeLevel(1, false);

                            foreach (Ability learnedAbility in comp.LearnedAbilities)
                                abilitySelectionList.Remove(learnedAbility.def);
                        }
                    }
                }

                implant.ImproveStats(psycastExtension.statUpgradePoints.RandomInRange);

            }
        }
    }
}
