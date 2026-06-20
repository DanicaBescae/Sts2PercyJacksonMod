using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class HiddenReservoir : PercyJacksonCard
{
    private int IncreasedDamage
    {
        get;
        set
        {
            AssertMutable();
            field = value;
        }
    }
    
    public HiddenReservoir() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9);
        WithVar("DamageIncrease", 5, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }

    public override Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler != Owner) return Task.CompletedTask;
        DynamicVars.Damage.BaseValue += DynamicVars["DamageIncrease"].BaseValue;
        IncreasedDamage += DynamicVars["DamageIncrease"].IntValue;
        return Task.CompletedTask;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        DynamicVars.Damage.BaseValue += IncreasedDamage;
    }
}