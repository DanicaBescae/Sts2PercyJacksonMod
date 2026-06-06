using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Common;


public class RideTheWave: PercyJacksonCard
{
    public RideTheWave() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithTide(2, 1);
        WithCards(1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue);
        await CommonActions.Draw(this, choiceContext);
    }
}