using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class LetItBe : PercyJacksonCard
{
    public LetItBe() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.Apply<LetItBePower>(choiceContext, Owner.Creature, this,
            DynamicVars.Cards.BaseValue);
    }
}