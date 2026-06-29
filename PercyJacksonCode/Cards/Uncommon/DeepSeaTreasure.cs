using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class DeepSeaTreasure : PercyJacksonCard
{
    public DeepSeaTreasure() : base(2, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(typeof(Water));
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<DeepSeaTreasurePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}