using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Hydrogenesis : PercyJacksonCard
{
    public Hydrogenesis() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(Water));
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithVar("Stacks", 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<HydrogenesisPower>(choiceContext, Owner.Creature, DynamicVars["Stack"].BaseValue,
            Owner.Creature, this);
    }
}