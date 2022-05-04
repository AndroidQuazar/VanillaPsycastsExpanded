namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Linq;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_IceWall : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            var cells = GenRadial.RadialCellsAround(target.Cell, 5, 5.9f);
            AbilityExtension_Building extension = this.def.GetModExtension<AbilityExtension_Building>();
            foreach (var cell in cells)
            {
                Thing building = GenSpawn.Spawn(extension.building, cell, this.pawn.Map);
                building.SetFactionDirect(this.pawn.Faction);
            }
        }
    }
}