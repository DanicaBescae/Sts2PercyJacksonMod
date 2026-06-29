using System.Runtime.ExceptionServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SlashEm : PercyJacksonCard
{
    public SlashEm() : base(0, CardType.Attack,
        CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4);
        WithPower<WoundedPower>(1, 1);
    }
    
    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var attackContext = await AttackCommand.CreateContextAsync(CombatState, choiceContext, this);
        try
        {
            for (var attackCount = ResolveEnergyXValue(); attackCount > 0; --attackCount)
            {
                await PowerCmd.Apply<WoundedPower>(choiceContext, play.Target, DynamicVars["WoundedPower"].BaseValue,
                    Owner.Creature, this);
                var damageResults = await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.Damage,
                    Owner.Creature,
                    this);
                attackContext.AddHit(damageResults);
            }
        }
        catch (Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex).Throw();
        }
        await attackContext.DisposeAsync();
    }
}