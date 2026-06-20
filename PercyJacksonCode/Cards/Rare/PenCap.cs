using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class PenCap : PercyJacksonCard
{
    public PenCap() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithComboStarter();
        WithKeyword(CardKeyword.Innate);
        WithDamage(5, 3);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }

    public override bool TryModifyCardRewardOptions(Player player, List<CardCreationResult> cardRewardOptions, CardCreationOptions creationOptions)
    {
        var penCaps = cardRewardOptions.Where(result => result.Card is PenCap).ToList();
        if (penCaps.Count == 0) return false;
        foreach (var penCap in penCaps)
        {
            var card = player.RunState.CloneCard(penCap.Card);
            CardCmd.Enchant<PerfectFit>(card, 1);
            penCap.ModifyCard(card);
        }

        return false;
    }

    public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
    {
        if (card is not PenCap)
        {
            newCard = card;
            return false;
        }
        var penCap = card.Owner.RunState.CloneCard(card);
        CardCmd.Enchant<PerfectFit>(penCap, 1);
        newCard = penCap;
        return true;
    }

    public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
    {
        var penCaps = cards.Where(result => result.Card is PenCap).ToList();
        if (penCaps.Count == 0) return;
        foreach (var penCap in penCaps)
        {
            var card = player.RunState.CloneCard(penCap.Card);
            CardCmd.Enchant<PerfectFit>(card, 1);
            penCap.ModifyCard(card);
        }
    }

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card is not PenCap) return Task.CompletedTask;
        CardCmd.Enchant<PerfectFit>(card, 1);
        return Task.CompletedTask;
    }
}