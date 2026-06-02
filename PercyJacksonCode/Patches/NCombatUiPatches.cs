using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.Combat;
using PercyJackson.PercyJacksonCode.Fields;

namespace PercyJackson.PercyJacksonCode.Patches;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
internal class NCombatUiPatches
{
    [HarmonyPostfix]
    private static void Postfix(NCombatUi __instance, CombatState state)
    {
        var tideCounter = PercyJacksonFields.TideCounter[__instance];
        if (tideCounter == null) return;
        tideCounter.Initialize(LocalContext.GetMe(state));
        tideCounter.Reparent(__instance.EnergyCounterContainer);
        tideCounter.Position = new Vector2(0, -120);
        tideCounter.Size = new Vector2(128, 128);
    }
}