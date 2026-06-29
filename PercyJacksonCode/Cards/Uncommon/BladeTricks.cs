using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class BladeTricks : PercyJacksonCard
{
    public BladeTricks() : base(1, CardType.Power,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(ComboKeyword);
        WithTip(CardKeyword.Retain);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BladeTricksPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}