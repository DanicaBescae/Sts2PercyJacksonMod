using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class GreekStyleImprov: PercyJacksonCard
{
    public GreekStyleImprov() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(ComboKeyword);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<GreekStyleImprovPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}