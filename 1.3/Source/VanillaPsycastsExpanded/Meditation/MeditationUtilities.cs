namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using Verse;

    public static class MeditationUtilities
    {
        [DebugAction("Pawns", "Check Meditation Focus Strength", true, actionType = DebugActionType.ToolMapForPawns,
                     allowedGameStates                                            = AllowedGameStates.PlayingOnMap)]
        public static void CheckStrength(Pawn pawn)
        {
            LocalTargetInfo focus = MeditationUtility.BestFocusAt(pawn.Position, pawn);
            if (!focus.HasThing) return;
            float value = focus.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn);
            Log.Message(
                $"Value: {value}, Explanation:\n{StatDefOf.MeditationFocusStrength.Worker.GetExplanationFull(StatRequest.For(focus.Thing, pawn), ToStringNumberSense.Absolute, value)}");
        }
    }
}