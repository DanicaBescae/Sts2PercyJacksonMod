using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Powers;

public class NoSweatPower : PercyJacksonPower, IAfterComboIncreased
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    private const int CardsBeforeEnergy = 4;

    public async Task AfterComboIncreased(PlayerChoiceContext choiceContext, Player player, int newCombo)
    {
        if (player != Owner.Player) return;
        if (newCombo % CardsBeforeEnergy == 0) await PlayerCmd.GainEnergy(Amount, player);
    }
}