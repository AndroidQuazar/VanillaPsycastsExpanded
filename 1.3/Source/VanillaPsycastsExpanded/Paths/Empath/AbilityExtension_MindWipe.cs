namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using Verse;
using VFECore;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class AbilityExtension_MindWipe : AbilityExtension_AbilityMod
	{
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            var pawn = target.Pawn;
            if (pawn.Faction != ability.pawn.Faction)
            {
                pawn.SetFaction(ability.pawn.Faction);
            }
            pawn.needs.mood.thoughts.memories.Memories.Clear();
            pawn.relations.ClearAllRelations();
            var passions = new Dictionary<SkillDef, Passion>();
            foreach (var skillRecord in pawn.skills.skills)
            {
                passions[skillRecord.def] = skillRecord.passion;
            }
            pawn.skills = new Pawn_SkillTracker(pawn);
            NonPublicMethods.GenerateSkills(pawn);
            foreach (var kvp in passions)
            {
                pawn.skills.GetSkill(kvp.Key).passion = kvp.Value;
            }
        }
    }
}