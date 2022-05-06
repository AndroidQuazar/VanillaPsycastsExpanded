namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using Verse;
    public class GameComponent_PsycastsManager : GameComponent
    {
        public List<GoodwillImpactDelayed> goodwillImpacts = new List<GoodwillImpactDelayed>();
        public GameComponent_PsycastsManager(Game game)
        {

        }

        public override void GameComponentTick()
        {
            base.GameComponentTick();
            for (var i = goodwillImpacts.Count - 1; i >= 0; i--)
            {
                var goodwillImpact = goodwillImpacts[i];
                if (Find.TickManager.TicksGame >= goodwillImpact.impactInTicks)
                {
                    goodwillImpact.DoImpact();
                    goodwillImpacts.RemoveAt(i);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref goodwillImpacts, "goodwillImpacts", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                goodwillImpacts ??= new List<GoodwillImpactDelayed>();
            }
        }
    }
}