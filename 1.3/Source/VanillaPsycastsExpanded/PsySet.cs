using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaPsycastsExpanded
{
    using Verse;
    using VFECore.Abilities;

    public class PsySet : IExposable
    {
        private string name;

        private List<AbilityDef> abilities = new List<AbilityDef>();

        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        public List<AbilityDef> Abilities
        {
            get => this.abilities;
            set => this.abilities = value;
        }

        public void AddAbility(AbilityDef ability)
        {
            this.abilities.Add(ability);
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref name, nameof(this.name));
            Scribe_Collections.Look(ref this.abilities, nameof(this.abilities));
        }
    }
}
