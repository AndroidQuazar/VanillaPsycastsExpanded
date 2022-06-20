﻿namespace VanillaPsycastsExpanded.Nightstalker
{
    using Verse;

    public class HediffComp_DissapearsInLight : HediffComp
    {
        public override bool CompShouldRemove => this.Pawn.Map.glowGrid.GameGlowAt(this.Pawn.Position) >= 0.21f;
    }

    public class HediffComp_DissapearsOnAttack : HediffComp
    {
        public override bool CompShouldRemove => this.Pawn.stances.curStance is Stance_Warmup {ticksLeft: <= 1, verb: {verbProps: {violent: true}}};
    }
}