using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SurfaceTension: PercyJacksonCard
{
    public SurfaceTension() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTide(3, 1);
    }

    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        TideManager.UpdateMaxTide(Owner, DynamicVars["Tide"].IntValue);
        return Task.CompletedTask;
    }
}