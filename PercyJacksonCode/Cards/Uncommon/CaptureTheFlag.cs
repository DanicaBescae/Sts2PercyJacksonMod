using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class CaptureTheFlag: PercyJacksonCard
{
    public CaptureTheFlag() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(5,2);
        WithTide(1);
        WithVar("HitCount", 1);
        WithVar("HitIncrease", 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.BaseValue, ValueProp.Move,
            DynamicVars["HitCount"].IntValue).Execute(choiceContext);
        DynamicVars["HitCount"].BaseValue += DynamicVars["HitIncrease"].IntValue;
    }
}