using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class StLouisArch: PercyJacksonCard
{
    public StLouisArch() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithTide(1);
        WithBlock(15, 5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue, true);
        await CommonActions.CardBlock(this, play);
    }
}