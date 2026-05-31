using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IAfterComboStarted
{
    public Task AfterComboStarted(PlayerChoiceContext choiceContext, CardModel card);
}