using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SeasEmbrace : PercyJacksonCard
{
    public SeasEmbrace() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<VigorPower>(2, 4);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var debuffsToRemove = Owner.Creature.Powers.Where(p => p is { Type: PowerType.Debuff, StackType: PowerStackType.Counter });
        var powerModels = debuffsToRemove as PowerModel[] ?? debuffsToRemove.ToArray();
        var totalDebuffs = powerModels.Length;
        for (var i = 0; i < totalDebuffs; i++)
        {
            await PowerCmd.Remove(powerModels.ElementAt(0));
        }

        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue, Owner.Creature, this);
    }
}