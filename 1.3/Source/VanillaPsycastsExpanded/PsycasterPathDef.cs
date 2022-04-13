using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaPsycastsExpanded
{
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class PsycasterPathDef : Def
    {
        public           string    background;
        [Unsaved] 
        public Texture2D backgroundImage;

        [Unsaved]
        public List<AbilityDef> abilities;

        public string tab;
        public int    order;

        public override void PostLoad()
        {
            base.PostLoad();
            LongEventHandler.ExecuteWhenFinished(delegate
                                                 {
                                                     this.backgroundImage = ContentFinder<Texture2D>.Get(this.background);
                                                 });
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            this.abilities = new List<AbilityDef>();

            foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefsListForReading)
            {
                AbilityExtension_Psycast psycast = abilityDef.GetModExtension<AbilityExtension_Psycast>();
                if(psycast.path == this)
                    this.abilities.Add(abilityDef);
            }
        }
    }
}
