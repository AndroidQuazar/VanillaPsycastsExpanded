using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
    using HarmonyLib;
    using Verse;

    [StaticConstructorOnStartup]
    public static class HarmonyInit
    {
        static HarmonyInit()
        {
            Harmony harmony = new Harmony("OskarPotocki.VanillaPsycastsExpanded");
            harmony.PatchAll();
        }
    }
}
