namespace VanillaPsycastsExpanded
{
    using System.Globalization;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    public class Command_Ability_Psycast : Command_Ability
    {
        private AbilityExtension_Psycast psycastExtension;

        public Command_Ability_Psycast(Pawn pawn, Ability ability) : base(pawn, ability) => 
            this.psycastExtension = this.ability.def.GetModExtension<AbilityExtension_Psycast>();

        public override string TopRightLabel
        {
            get
            {
                string topRightLabel = string.Empty;
                float  entropy       = this.psycastExtension.GetEntropyUsedByPawn(this.ability.pawn);
                if (entropy > float.Epsilon)
                    topRightLabel += "NeuralHeatLetter".Translate() + ": " + entropy.ToString(CultureInfo.CurrentCulture) + "\n";

                float psyfocusCost = this.psycastExtension.GetPsyfocusUsedByPawn(this.ability.pawn);
                if (psyfocusCost > float.Epsilon)
                    topRightLabel += "PsyfocusLetter".Translate() + ": " + psyfocusCost.ToStringPercent();
                return topRightLabel.TrimEndNewlines();
            }
        }
    }
}
