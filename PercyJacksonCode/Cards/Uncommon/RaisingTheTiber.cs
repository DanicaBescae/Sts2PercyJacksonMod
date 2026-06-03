using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class RaisingTheTiber: PercyJacksonCard
{
    public RaisingTheTiber() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithTide(1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<RaisingTheTiberPower>(choiceContext, Owner.Creature, DynamicVars["Tide"].IntValue,
            Owner.Creature, this);
    }
}