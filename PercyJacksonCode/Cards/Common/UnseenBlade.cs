using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Cards.Common;

public class UnseenBlade: PercyJacksonCard
{
    public UnseenBlade() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithVar("IncreasedEnergy", 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move).Execute(choiceContext);
        var clone = CreateClone();
        clone.EnergyCost.SetThisCombat(EnergyCost.GetAmountToSpend() + DynamicVars["IncreasedEnergy"].IntValue);
        await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, Owner);
    }
}