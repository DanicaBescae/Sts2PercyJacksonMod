using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class SwordMasteryPower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    public decimal ModifyComboMultiplier(
        Creature target,
        decimal amount,
        decimal comboCount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target == Owner || props != ValueProp.Move || dealer != Owner
            ? amount
            : amount + (Amount * comboCount) / 100M;
    }

    
}