namespace VanillaPsycastsExpanded.Staticlord
{
    using RimWorld;
    using Verse;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Thunderbolt : Ability
    {
        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            foreach (Thing thing in target.Cell.GetThingList(this.pawn.Map))
                thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 25, -1f, this.pawn.DrawPos.AngleToFlat(thing.DrawPos), this.pawn));
            GenExplosion.DoExplosion(target.Cell, this.pawn.Map, this.GetRadiusForPawn(), DamageDefOf.EMP, this.pawn);
            this.pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(this.pawn.Map, target.Cell));
        }
    }
}