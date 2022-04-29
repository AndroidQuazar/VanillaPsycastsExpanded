namespace VanillaPsycastsExpanded.Harmonist
{
    using System.Linq;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Skillroll : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Pawn pawn   = target.Pawn;
            int  points = Mathf.FloorToInt(pawn.skills.skills.Sum(skill => skill.Level) * 1.1f);

            foreach (SkillRecord skill in pawn.skills.skills) skill.levelInt = 0;
            for (int i = 0; i < points; i++)
            {
                SkillRecord skill = pawn.skills.skills.Where(skill => !skill.TotallyDisabled && skill.levelInt < 20).RandomElement();
                skill.levelInt++;
            }
        }

        public override bool CanHitTarget(LocalTargetInfo target) =>
            base.CanHitTarget(target) && target.Pawn is {Faction: { } targetFaction} && this.pawn is {Faction: { } faction} &&
            (faction == targetFaction || faction.RelationKindWith(targetFaction) == FactionRelationKind.Ally);
    }
}