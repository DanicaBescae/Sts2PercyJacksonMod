using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PercyJackson.PercyJacksonCode.Hooks;

public interface IAfterComboEnded
{
    public Task AfterComboEnded(PlayerChoiceContext choiceContext, int combo);
}