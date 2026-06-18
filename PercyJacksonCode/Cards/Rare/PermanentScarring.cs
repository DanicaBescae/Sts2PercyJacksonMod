using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class PermanentScarring : PercyJacksonCard
{
    public PermanentScarring() : base(2, CardType.Power,
        CardRarity.Rare, TargetType.Self)
    {
        WithTip(typeof(WoundedPower));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<PermanentScarringPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}