namespace VanillaPsycastsExpanded.Staticlord
{
    using HarmonyLib;
    using RimWorld;
    using UnityEngine;
    using Verse;
    using Ability = VFECore.Abilities.Ability;
    using Command_Ability = VFECore.Abilities.Command_Ability;

    public class HurricaneMaker : Thing
    {
        private GameCondition caused;
        public  Pawn          Pawn;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.caused                 = GameConditionMaker.MakeConditionPermanent(VPE_DefOf.VPE_Hurricane_Condition);
            this.caused.conditionCauser = this;
            map.GameConditionManager.RegisterCondition(this.caused);
            this.Map.weatherManager.TransitionTo(VPE_DefOf.VPE_Hurricane_Weather);
            this.Map.weatherDecider.StartNextWeather();
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            this.caused.End();
            this.Map.weatherDecider.StartNextWeather();
            base.Destroy(mode);
        }

        public override void Tick()
        {
            if (!this.Pawn.psychicEntropy.TryAddEntropy(1f, this)) this.Destroy();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref this.caused, "caused");
            Scribe_References.Look(ref this.Pawn,   "pawn");
        }
    }

    public class Ability_Hurricane : Ability, IAbilityToggle
    {
        private HurricaneMaker maker;

        public bool Toggle
        {
            get => this.maker is {Destroyed: false};
            set
            {
                if (value)
                    this.DoAction();
                else
                    this.maker?.Destroy();
            }
        }

        public string OffLabel => "VPE.StopHurricane".Translate();

        public override void Cast(LocalTargetInfo target)
        {
            base.Cast(target);
            this.maker      = (HurricaneMaker) ThingMaker.MakeThing(VPE_DefOf.VPE_HurricaneMaker);
            this.maker.Pawn = this.pawn;
            GenSpawn.Spawn(this.maker, this.pawn.Position, this.pawn.Map);
        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref this.maker, nameof(this.maker));
        }

        public override Gizmo GetGizmo() => new Command_AbilityToggle(this.pawn, this);
    }

    public interface IAbilityToggle
    {
        public bool   Toggle   { get; set; }
        public string OffLabel { get; }
    }

    public class Command_AbilityToggle : Command_Ability
    {
        public Command_AbilityToggle(Pawn pawn, Ability ability) : base(pawn, ability)
        {
            if (this.Toggle.Toggle)
            {
                this.disabled       = false;
                this.disabledReason = null;
            }
        }

        public IAbilityToggle Toggle => this.ability as IAbilityToggle;

        public override string Label => this.Toggle.Toggle ? this.Toggle.OffLabel : base.Label;

        public override void ProcessInput(Event ev)
        {
            this.Toggle.Toggle = !this.Toggle.Toggle;
        }
    }

    [StaticConstructorOnStartup]
    [HarmonyPatch]
    public class WeatherOverlay_RainSideways : SkyOverlay
    {
        private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld");
        private                 float    rotation;

        public WeatherOverlay_RainSideways()
        {
            this.worldOverlayMat       = RainOverlayWorld;
            this.worldOverlayPanSpeed1 = 0.015f;
            this.worldPanDir1          = new Vector2(-1f, -0.25f);
            this.worldPanDir1.Normalize();
            this.worldOverlayPanSpeed2 = 0.022f;
            this.worldPanDir2          = new Vector2(-1f, -0.22f);
            this.worldPanDir2.Normalize();
        }

        public override void TickOverlay(Map map)
        {
            base.TickOverlay(map);
            this.rotation = Mathf.Clamp(this.rotation + 1f, 0f, 360f);
        }

        // [HarmonyPatch(typeof(SkyOverlay), nameof(DrawOverlay))]
        // [HarmonyTranspiler]
        // public static IEnumerable<CodeInstruction> DrawOverlyTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        // {
        //     MethodInfo info = AccessTools.PropertyGetter(typeof(Quaternion), nameof(Quaternion.identity));
        //     foreach (CodeInstruction instruction in instructions)
        //         if (instruction.Calls(info))
        //         {
        //             Label label  = generator.DefineLabel();
        //             Label label2 = generator.DefineLabel();
        //             yield return new CodeInstruction(OpCodes.Ldarg_0);
        //             yield return new CodeInstruction(OpCodes.Isinst,  typeof(WeatherOverlay_RainSideways));
        //             yield return new CodeInstruction(OpCodes.Brfalse, label);
        //             yield return new CodeInstruction(OpCodes.Ldsfld,  AccessTools.Field(typeof(WeatherOverlay_RainSideways), nameof(rotation)));
        //             yield return new CodeInstruction(OpCodes.Call,    AccessTools.PropertyGetter(typeof(Vector3), nameof(Vector3.up)));
        //             yield return new CodeInstruction(OpCodes.Call,    AccessTools.Method(typeof(Quaternion), nameof(Quaternion.AngleAxis)));
        //             yield return new CodeInstruction(OpCodes.Br,      label2);
        //             yield return instruction.WithLabels(label);
        //             yield return new CodeInstruction(OpCodes.Nop).WithLabels(label2);
        //         }
        //         else yield return instruction;
        // }
    }
}