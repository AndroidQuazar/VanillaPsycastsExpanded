namespace VanillaPsycastsExpanded.Technomancer
{
    using System.Collections.Generic;
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    [StaticConstructorOnStartup]
    public class Ability_Construct_Rock : Ability
    {
        private static readonly HashSet<ThingDef> chunkCache = new();

        static Ability_Construct_Rock()
        {
            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
                if (def.IsNonResourceNaturalRock)
                    if (def.building.mineableThing != null)
                        chunkCache.Add(def.building.mineableThing);
        }

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            Pawn construct = PawnGenerator.GeneratePawn(VPE_DefOf.VPE_RockConstruct, this.pawn.Faction);
            construct.TryGetComp<CompBreakLink>().Pawn = this.pawn;
            this.pawn.Psycasts().OffsetMinHeat(20f);
            construct.TryGetComp<CompSetStoneColour>().SetStoneColour(target.Thing.def);
            GenSpawn.Spawn(construct, target.Thing.Position, target.Thing.Map, target.Thing.Rotation);
            target.Thing.Destroy();
        }

        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (!base.ValidateTarget(target, showMessages)) return false;
            if (!target.HasThing) return false;
            if (!chunkCache.Contains(target.Thing.def))
            {
                if (showMessages) Messages.Message("VPE.MustBeStoneChunk".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            if (this.pawn.psychicEntropy.MaxEntropy - this.pawn.psychicEntropy.EntropyValue <= 20f)
            {
                if (showMessages) Messages.Message("VPE.NotEnoughHeat".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            return true;
        }
    }
}