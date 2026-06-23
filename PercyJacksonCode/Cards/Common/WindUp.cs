using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class WindUp: PercyJacksonCard
{
    public WindUp() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithComboStarter();
        WithPower<VulnerablePower>(1, 1);
        WithPower<VigorPower>(2, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<VigorPower>(choiceContext, play.Target, DynamicVars["VigorPower"].IntValue, Owner.Creature, this);
    }
}