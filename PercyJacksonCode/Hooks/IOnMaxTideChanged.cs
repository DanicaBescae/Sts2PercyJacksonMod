using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IOnMaxTideChanged
{
    public Task OnMaxTideChanged(PlayerChoiceContext choiceContext, Player player, bool fromOverflow = false);
}