using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IBeforeWaterActivated
{
    public Task BeforeWaterActivated(PlayerChoiceContext choiceContext, CardModel card);
}