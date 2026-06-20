using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class RainbowHorse : PercyJacksonCard, IAfterWaterActivated
{
    public RainbowHorse() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(13, 3);
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    public Task AfterWaterActivated(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner) return Task.CompletedTask;
        EnergyCost.SetUntilPlayed(0);
        return Task.CompletedTask;
    }
}