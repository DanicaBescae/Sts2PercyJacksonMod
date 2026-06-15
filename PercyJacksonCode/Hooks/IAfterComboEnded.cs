using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IAfterComboEnded
{
    public Task AfterComboEnded(PlayerChoiceContext choiceContext, Player player, int combo);
}