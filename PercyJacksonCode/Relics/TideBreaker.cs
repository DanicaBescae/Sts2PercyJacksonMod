using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Relics;

namespace PercyJackson.PercyJacksonCode.Relics;

public class TideBreaker() : PercyJacksonRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Ancient;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(PercyJacksonCard.TideKeyword)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber > 1) return;
        TideManager.UpdateMaxTide(player, 3);
        await TideManager.UpdateTide(player, 3);
    }
}