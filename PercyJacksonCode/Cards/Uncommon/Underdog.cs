using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Underdog: PercyJacksonCard
{
    public Underdog() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 11);
        WithPower<VulnerablePower>(2);
    }
    
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            return CombatState != null && CombatState.HittableEnemies.Any (e =>
            {
                var monster = e.Monster;
                return monster is { IntendsToAttack: false };
            });
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move)
            .Execute(choiceContext);
        if (play.Target.Monster is { IntendsToAttack: true })
            return;
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars["VulnerablePower"].BaseValue,
            Owner.Creature, this);
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        return cardSource != this || target == null || dealer != Owner.Creature || !props.IsPoweredAttack() ||
               target.Monster is { IntendsToAttack: true }
            ? 1M
            : 2M;
    }
}