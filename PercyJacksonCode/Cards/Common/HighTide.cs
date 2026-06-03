using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class HighTide: PercyJacksonCard
{
    public HighTide() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithTide(3, 5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        TideManager.UpdateMaxTide(Owner, DynamicVars["Tide"].IntValue, true);
    }
}