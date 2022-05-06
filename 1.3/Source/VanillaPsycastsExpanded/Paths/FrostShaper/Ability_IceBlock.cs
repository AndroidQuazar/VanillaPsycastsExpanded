namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Linq;
    using Verse;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;
    public class Ability_IceBlock : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            var cells = CellRect.CenteredOn(target.Cell, 5, 5).Cells.InRandomOrder().ToList();
            cells = cells.Take(cells.Count() - 5).ToList();
            AbilityExtension_Building extension = this.def.GetModExtension<AbilityExtension_Building>();
            foreach (var cell in cells)
            {
                if (cell.GetEdifice(this.pawn.Map) is null)
                {
                    Thing building = GenSpawn.Spawn(extension.building, cell, this.pawn.Map);
                    building.SetFactionDirect(this.pawn.Faction);
                }
            }
        }
    }
}