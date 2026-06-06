using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class GettingSerious: PercyJacksonCard {

    public GettingSerious() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithTips(_ => [HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<VigorPower>()]);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int strengthGain;
        if (IsUpgraded)
        {
            strengthGain = Owner.Creature.GetPowerAmount<VigorPower>();
        }
        else
        {
            strengthGain = Owner.Creature.GetPowerAmount<VigorPower>() / 2;
        }

        await PowerCmd.ModifyAmount(choiceContext, Owner.Creature.GetPower<VigorPower>(), -1 * strengthGain,
            Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, strengthGain, Owner.Creature, this);
    }
}