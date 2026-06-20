using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class YouDrool : PercyJacksonCard
{
    public YouDrool() : base(1, CardType.Skill,
        CardRarity.Common, TargetType.Self)
    {
        WithEnergy(2, 1);
        WithPower<VigorPower>(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.IntValue,
            Owner.Creature, this);
        await PowerCmd.Apply<VigorNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue,
            Owner.Creature, this);
    }
}