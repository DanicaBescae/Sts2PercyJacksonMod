using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class SurfacedTreasurePower() : PercyJacksonPower, IBeforeWaterActivated
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public async Task BeforeWaterActivated(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner.Player) return;
        Flash();
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
}