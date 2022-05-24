﻿namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using Verse;

    public class GameComponent_PsycastsManager : GameComponent
    {
        public  List<GoodwillImpactDelayed>   goodwillImpacts  = new();
        public  List<(Thing thing, int tick)> removeAfterTicks = new();
        private List<Thing>                   removeAfterTicks_things;
        private List<int>                     removeAfterTicks_ticks;

        public GameComponent_PsycastsManager(Game game)
        {
        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            for (int i = this.goodwillImpacts.Count - 1; i >= 0; i--)
            {
                GoodwillImpactDelayed goodwillImpact = this.goodwillImpacts[i];
                if (Find.TickManager.TicksGame >= goodwillImpact.impactInTicks)
                {
                    goodwillImpact.DoImpact();
                    this.goodwillImpacts.RemoveAt(i);
                }
            }

            for (int i = this.removeAfterTicks.Count - 1; i >= 0; i--)
            {
                Thing thing = this.removeAfterTicks[i].thing;
                int   tick  = this.removeAfterTicks[i].tick;
                Log.Message($"Checking: thing={thing}, tick={tick}, curTick={Find.TickManager.TicksGame}");
                if (thing is null or {Destroyed: true})
                    this.removeAfterTicks.RemoveAt(i);
                else if (Find.TickManager.TicksGame >= tick)
                {
                    thing.Destroy();
                    this.removeAfterTicks.RemoveAt(i);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref this.goodwillImpacts, "goodwillImpacts", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit) this.goodwillImpacts ??= new List<GoodwillImpactDelayed>();

            if (Scribe.mode == LoadSaveMode.Saving)
            {
                this.removeAfterTicks_things = new List<Thing>();
                this.removeAfterTicks_ticks  = new List<int>();
                for (int i = 0; i < this.removeAfterTicks.Count; i++)
                {
                    this.removeAfterTicks_things.Add(this.removeAfterTicks[i].thing);
                    this.removeAfterTicks_ticks.Add(this.removeAfterTicks[i].tick);
                }
            }

            Scribe_Collections.Look(ref this.removeAfterTicks_things, "removeAfterTick_things", LookMode.Reference);
            Scribe_Collections.Look(ref this.removeAfterTicks_ticks,  "removeAfterTick_ticks",  LookMode.Value);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                this.removeAfterTicks = new List<(Thing thing, int tick)>();
                for (int i = 0; i < this.removeAfterTicks_things.Count; i++)
                    this.removeAfterTicks.Add((this.removeAfterTicks_things[i], this.removeAfterTicks_ticks[i]));
            }
        }
    }
}