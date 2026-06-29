using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class WaitForIt : PercyJacksonCard
{
    public WaitForIt() : base(2, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(1, 1);
        WithTip(CardKeyword.Retain);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<RetainHandPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}