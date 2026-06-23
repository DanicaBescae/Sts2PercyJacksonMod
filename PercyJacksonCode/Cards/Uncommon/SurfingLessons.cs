using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SurfingLessons: PercyJacksonCard
{
    private bool _needCheckDamageDealt;
    
    public SurfingLessons() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithCalculatedDamage(0, (c, _) => c.Owner.PlayerCombatState.Tide().TideGainedThisTurn);
        WithKeyword(TideKeyword);
        WithCombo(2);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsComboComplete(this)) _needCheckDamageDealt = true;
        await CommonActions.CardAttack(this, cardPlay.Target, DynamicVars.CalculatedDamage.BaseValue, ValueProp.Move,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (!_needCheckDamageDealt || dealer != Owner.Creature || cardSource != this) return;
        await TideManager.UpdateTide(Owner, result.UnblockedDamage);
        _needCheckDamageDealt = false;
    }
}