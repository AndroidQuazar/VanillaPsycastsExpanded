namespace VanillaPsycastsExpanded
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class PsycasterPathDef : Def
    {
        public static    AbilityDef       Blank;
        public static    string           BlankLabel = "$$Blank$$";
        [Unsaved] public List<AbilityDef> abilities;

        public AbilityDef[][] abilityLevelsInOrder;

        public string background;

        [Unsaved] public Texture2D backgroundImage;

        public int    order;
        public string tab;

        [Unsaved] public bool HasAbilities;
        [Unsaved] public int  MaxLevel;

        public override void PostLoad()
        {
            base.PostLoad();
            LongEventHandler.ExecuteWhenFinished(delegate { this.backgroundImage = ContentFinder<Texture2D>.Get(this.background); });
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            Blank = new AbilityDef {defName = BlankLabel};

            this.abilities = new List<AbilityDef>();
            foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefsListForReading)
            {
                AbilityExtension_Psycast psycast = abilityDef.GetModExtension<AbilityExtension_Psycast>();
                if (psycast is not null && psycast.path == this) this.abilities.Add(abilityDef);
            }

            this.MaxLevel             = this.abilities.Max(ab => ab.Psycast().level);
            this.abilityLevelsInOrder = new AbilityDef[this.MaxLevel][];
            foreach (IGrouping<int, AbilityDef> abilityDefs in this.abilities.GroupBy(ab => ab.Psycast().level))
                this.abilityLevelsInOrder[abilityDefs.Key - 1] = abilityDefs.OrderBy(ab => ab.Psycast().order)
                                                                            .SelectMany(ab => ab.Psycast().spaceAfter
                                                                                            ? new List<AbilityDef> {ab, Blank}
                                                                                            : Gen.YieldSingle(ab)).ToArray();

            this.HasAbilities = this.abilityLevelsInOrder.Any(arr => !arr.NullOrEmpty());
            if (!this.HasAbilities) return;
            Log.Message($"Abilities for {this.label}:");
            for (int i = 0; i < this.abilityLevelsInOrder.Length; i++)
            {
                AbilityDef[] arr = this.abilityLevelsInOrder[i];
                if (arr is null)
                    this.abilityLevelsInOrder[i] = new AbilityDef[0];
                else
                {
                    Log.Message($"  Level {i + 1}:");
                    for (int j = 0; j < arr.Length; j++) Log.Message($"    {j}: {arr[j].label}");
                }
            }
        }
    }
}