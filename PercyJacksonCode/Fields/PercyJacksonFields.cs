using BaseLib.Utils;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Nodes;
using PercyJackson.PercyJacksonCode.Utils;

namespace PercyJackson.PercyJacksonCode.Fields;

public class PercyJacksonFields
{
    public static readonly SpireField<PlayerCombatState, PlayerCombatStateExtensions.TideCombatState> TideCombatState = new(()=>null);
    public static readonly SpireField<PlayerCombatState, PlayerCombatStateExtensions.ComboCombatState> ComboCombatState = new(()=>null);
    public static readonly AddedNode<NCombatUi, NTideCounter> TideCounter = new((ui) =>
    {
        var tideCounter = PreloadManager.Cache.GetScene(PercyJacksonResource.NTideCounterPath)
            .Instantiate<NTideCounter>();
        ui.AddChildSafely(tideCounter);
        return tideCounter;
    });
    public static readonly AddedNode<NCombatUi, NComboDisplay> ComboDisplay = new((ui) =>
    {
        var comboDisplay = PreloadManager.Cache.GetScene(PercyJacksonResource.NComboDisplayPath)
            .Instantiate<NComboDisplay>();
        ui.AddChildSafely(comboDisplay);
        return comboDisplay;
    });
}