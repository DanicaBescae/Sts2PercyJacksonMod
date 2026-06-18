using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Thrust : PercyJacksonCard
{
    public Thrust() : base(2, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(12, 3);
        WithCombo(1);
        WithPower<WoundedPower>(4);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (IsComboComplete(this))
        {
            await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Unblockable)
                .Execute(choiceContext);
            await PowerCmd.Apply<WoundedPower>(choiceContext, play.Target, DynamicVars["WoundedPower"].BaseValue,
                Owner.Creature,
                this);
        }
        else
        {
            await CommonActions.CardAttack(this, play).Execute(choiceContext);
        }
    }
}