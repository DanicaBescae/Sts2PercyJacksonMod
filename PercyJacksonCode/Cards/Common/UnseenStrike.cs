using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class UnseenStrike: PercyJacksonCard
{
    public UnseenStrike() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        var clone = CreateClone();
        clone.EnergyCost.SetThisCombat(EnergyCost.GetAmountToSpend() + DynamicVars["IncreasedEnergy"].IntValue);
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, Owner);
    }
}