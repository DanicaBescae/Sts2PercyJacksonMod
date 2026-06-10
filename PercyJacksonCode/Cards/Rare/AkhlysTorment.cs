using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class AkhlysTorment: PercyJacksonCard
{
    public AkhlysTorment() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(Guilty));
        WithPower<TormentedPower>(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<TormentedPower>(choiceContext, play.Target, DynamicVars["TormentedPower"].BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<GuiltyPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}