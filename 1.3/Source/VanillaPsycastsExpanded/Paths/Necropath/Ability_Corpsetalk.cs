namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using RimWorld.Planet;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using Verse.AI;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_Corpsetalk : Ability_TargetCorpse
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            var hediff = this.ApplyHediff(this.pawn) as Hediff_CorpseTalk;
            hediff.skillXPDifferences = new Dictionary<SkillDef, int>();
            foreach (var target in targets)
            {
                var corpse = target.Thing as Corpse;
                foreach (var skillDef in DefDatabase<SkillDef>.AllDefs)
                {
                    var diff = corpse.InnerPawn.skills.GetSkill(skillDef).Level - pawn.skills.GetSkill(skillDef).Level;
                    if (diff > 0)
                    {
                        var oldValue = pawn.skills.GetSkill(skillDef).Level;
                        pawn.skills.GetSkill(skillDef).Level = Mathf.Min(20, pawn.skills.GetSkill(skillDef).Level + diff);
                        hediff.skillXPDifferences[skillDef] = pawn.skills.GetSkill(skillDef).Level - oldValue;
                    }
                }
            }
        }
    }
}