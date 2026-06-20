using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Ancient;

public class SonOfNeptune : PercyJacksonCard
{
    public SonOfNeptune() : base(1, CardType.Power,
        CardRarity.Ancient, TargetType.Self)
    {
        WithVar("Mult", 2);
        WithCalculatedVar("Decrease", 0, (c, _) => Math.Max(0, c.DynamicVars["Mult"].IntValue - 1));
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<SonOfNeptunePower>(choiceContext, Owner.Creature, DynamicVars["Mult"].BaseValue,
            Owner.Creature, this);
    }
}