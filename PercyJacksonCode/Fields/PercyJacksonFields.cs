using BaseLib.Utils;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Nodes;
using PercyJackson.PercyJacksonCode.Utils;

namespace PercyJackson.PercyJacksonCode.Fields;

public class PercyJacksonFields
{
    public static readonly SpireField<PlayerCombatState, TideCombatState> TideCombatState = new(()=>null);
    public static readonly AddedNode<NCombatUi, NTideCounter> TideCounter = new((ui) =>
    {
        var tideCounter = PreloadManager.Cache.GetScene(PercyJacksonResource.NTideCounterPath)
            .Instantiate<NTideCounter>();
        ui.AddChildSafely(tideCounter);
        return tideCounter;
    });
}