using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IAfterComboIncreased
{
    public Task AfterComboIncreased(PlayerChoiceContext choiceContext, Player player, int newCombo);
}