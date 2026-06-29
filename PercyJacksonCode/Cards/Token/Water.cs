using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using PercyJackson.PercyJacksonCode.Hooks;

namespace PercyJackson.PercyJacksonCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Water: ConstructedCardModel
{
    public Water() : base(-1, CardType.Status, CardRarity.Token, TargetType.Self)
    {
        WithKeyword(CardKeyword.Unplayable);
        WithKeyword(CardKeyword.Exhaust);
        WithCards(2);
    }

    public override int MaxUpgradeLevel => 0;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;
        await Activate(choiceContext);
        await CardCmd.Exhaust(choiceContext, this);
    }

    public async Task Activate(PlayerChoiceContext choiceContext)
    {
        await PercyJacksonHooks.BeforeWaterActivated(CombatState, choiceContext, this);
        await CommonActions.Draw(this, choiceContext);
        await PercyJacksonHooks.AfterWaterActivated(CombatState, choiceContext, this);
    }
}