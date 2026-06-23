using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;
using PercyJackson.PercyJacksonCode.Relics;

namespace PercyJackson.PercyJacksonCode.Relics;

public class PoseidonsBlessing() : PercyJacksonRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(PercyJacksonCard.TideKeyword)];

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber > 1) return Task.CompletedTask;
        TideManager.UpdateMaxTide(player, 3);
        return Task.CompletedTask;
    }
    
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Get<TideBreaker>().ToMutable();
    }
}