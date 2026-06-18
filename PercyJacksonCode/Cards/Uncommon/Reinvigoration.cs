using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Reinvigoration : PercyJacksonCard
{
    public Reinvigoration() : base(1, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(Water));
        WithPower<StrengthPower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<ReinvigorationPower>(choiceContext, Owner.Creature, DynamicVars["StrengthPower"].BaseValue,
            Owner.Creature, this);
    }
}