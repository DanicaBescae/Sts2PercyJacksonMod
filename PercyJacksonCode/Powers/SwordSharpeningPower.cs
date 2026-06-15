using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Powers;

public class SwordSharpeningPower: PercyJacksonPower, IAfterComboEnded
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (!props.IsPoweredAttack() || cardSource == null || dealer != Owner)
            return 0M;
        return ComboManager.WillIncreaseCombo(cardSource) ? Amount : 0M;
    }

    public async Task AfterComboEnded(PlayerChoiceContext choiceContext, Player player, int combo)
    {
        if (player.Creature != Owner) return;
        await PowerCmd.Remove(this);
    }
}