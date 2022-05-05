namespace VanillaPsycastsExpanded
{
    using Verse;
    using VFECore.Abilities;

    public class AbilityExtension_SpawnSnowAround : AbilityExtension_AbilityMod
    {
        public float radius;

        public float depth;
        public override void Cast(LocalTargetInfo target, Ability ability)
        {
            base.Cast(target, ability);
            foreach (var cell in GenRadial.RadialCellsAround(target.Cell, radius, true))
            {
                ability.pawn.Map.snowGrid.AddDepth(cell, depth);
            }
        }
    }
}