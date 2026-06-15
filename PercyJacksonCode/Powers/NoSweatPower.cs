using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Powers;

public class NoSweatPower : PercyJacksonPower, IAfterComboEnded
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public Task AfterComboEnded(PlayerChoiceContext choiceContext, Player player, int combo)
    {
        Flash();
        return PlayerCmd.GainEnergy(Amount, Owner.Player);
    }
}