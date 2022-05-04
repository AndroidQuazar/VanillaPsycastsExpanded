namespace VanillaPsycastsExpanded.Chronopath
{
    using System.Linq;
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_MaturePlants : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            foreach (Plant plant in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, this.GetRadiusForPawn(), true).OfType<Plant>())
            {
                plant.Growth = 1f;
                plant.DirtyMapMesh(plant.Map);
            }
        }
    }
}