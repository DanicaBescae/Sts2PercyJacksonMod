using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class TeamworkPower() : PercyJacksonPower, IAfterComboEnded
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(PercyJacksonCard.ComboKeyword)];

    public async Task AfterComboEnded(PlayerChoiceContext choiceContext, Player player, int combo)
    {
        if (player != Owner.Player) return;
        await CardPileCmd.Draw(choiceContext, Amount * combo, player);
    }
}