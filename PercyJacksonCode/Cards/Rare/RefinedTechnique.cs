using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class RefinedTechnique: PercyJacksonCard
{
    public RefinedTechnique() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCombo(6, -1);
        WithDamage(14);
        WithCalculatedVar("HitCount", 0, (_, _) => ComboManager.CurrentComboCount);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.Damage.BaseValue, ValueProp.Move,
            DynamicVars["HitCount"].IntValue).Execute(choiceContext);
        await ComboManager.ClearCombo(choiceContext, Owner.Creature.CombatState);
    }
}