namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class PsycasterPathDef : Def
    {
        [Unsaved] public List<AbilityDef> abilities;

        public AbilityDef[][] abilityLevelsInOrder;

        public string background;

        [Unsaved] public Texture2D backgroundImage;

        public int    order;
        public string tab;

        public override void PostLoad()
        {
            base.PostLoad();
            LongEventHandler.ExecuteWhenFinished(delegate { this.backgroundImage = ContentFinder<Texture2D>.Get(this.background); });
            this.abilityLevelsInOrder = new AbilityDef[5][];
            foreach (IGrouping<int, AbilityDef> abilityDefs in this.abilities.GroupBy(ab => ab.Psycast().level))
                this.abilityLevelsInOrder[abilityDefs.Key] = abilityDefs.OrderBy(ab => ab.Psycast().order).ToArray();
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            this.abilities = new List<AbilityDef>();

            foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefsListForReading)
            {
                AbilityExtension_Psycast psycast = abilityDef.GetModExtension<AbilityExtension_Psycast>();
                if (psycast.path == this)
                    this.abilities.Add(abilityDef);
            }
        }
    }
}