using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using PercyJackson.PercyJacksonCode.Fields;

namespace PercyJackson.PercyJacksonCode.Patches;

[HarmonyPatch(typeof(NCombatUi), nameof(NCombatUi.Activate))]
internal class NCombatUiPatches
{
    [HarmonyPostfix]
    private static void Postfix(NCombatUi __instance, CombatState state)
    {
        var tideCounter = PercyJacksonFields.TideCounter[__instance];
        var comboDisplay = PercyJacksonFields.ComboDisplay[__instance];
        
        var player = LocalContext.GetMe(state);
        if (player == null) return;
        
        if (tideCounter != null)
        {
            tideCounter.Initialize(player);
            tideCounter.Reparent(__instance.EnergyCounterContainer);
            tideCounter.Position = new Vector2(0, -120);
            tideCounter.Size = new Vector2(128, 128);
        }

        if (comboDisplay != null)
        {
            comboDisplay.Initialize(player);
            var vfxContainer = NCombatRoom.Instance.CombatVfxContainer;
            comboDisplay.Reparent(vfxContainer);

            var creatureNode = NCombatRoom.Instance.GetCreatureNode(player.Creature);
            if (creatureNode != null)
            {
                var globalTopPos = creatureNode.GetTopOfHitbox();
                comboDisplay.Position = vfxContainer.GetGlobalTransform().AffineInverse() * globalTopPos;
                comboDisplay.Position += new Vector2(100f, -80f);
                comboDisplay.Size = new Vector2(128, 128);
            }
        }
    }
}