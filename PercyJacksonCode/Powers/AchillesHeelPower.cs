using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PercyJackson.PercyJacksonCode.Powers;

public class AchillesHeelPower: PercyJacksonPower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Single;
    
    private const int EnergyGain = 2;

    public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        Flash();
        await PlayerCmd.GainEnergy(EnergyGain, Owner.Player);
        var debuffsToRemove =
            Owner.Powers.Where(p => p is { Type: PowerType.Debuff, StackType: PowerStackType.Counter });
        var powerModels = debuffsToRemove as PowerModel[] ?? debuffsToRemove.ToArray();
        var totalDebuffs = powerModels.Length;
        for (var i = 0; i < totalDebuffs; i++)
        {
            await PowerCmd.Remove(powerModels.ElementAt(0));
        }
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target != Owner || !props.IsPoweredAttack() ? 1M : 2M;
    }
}