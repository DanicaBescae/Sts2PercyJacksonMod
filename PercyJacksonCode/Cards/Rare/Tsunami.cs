using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Tsunami : PercyJacksonCard
{
    public Tsunami() : base(2, CardType.Attack,
        CardRarity.Rare, TargetType.RandomEnemy)
    {
        WithDamage(4, 2);
        WithCalculatedVar("HitCount", 0, (c, _) => GetUnplayables(c.Owner).Count());
        WithTip(CardKeyword.Exhaust);
        WithTip(CardKeyword.Unplayable);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var hitCount = DynamicVars["HitCount"].IntValue;
        var cardsToExhaust = GetUnplayables(Owner);
        foreach (var card in cardsToExhaust)
            await CardCmd.Exhaust(choiceContext, card);
        await CommonActions.CardAttack(this, play, hitCount: hitCount).Execute(choiceContext);
    }

    private static IEnumerable<CardModel> GetUnplayables(Player owner)
    {
        return owner.PlayerCombatState.AllCards.Where(c =>
            c.Keywords.Contains(CardKeyword.Unplayable) && c.Pile.Type != PileType.Exhaust);
    }
}