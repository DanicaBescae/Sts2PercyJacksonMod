using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Character;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class UnexpectedStrategy : PercyJacksonCard
{
    
    public UnexpectedStrategy() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithKeyword(CardKeyword.Exhaust);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        CardModel? card = CardFactory.GetDistinctForCombat(Owner, ModelDb.CardPool<PercyJacksonCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Tags.Contains(ComboTag)), 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (card == null)
            return;
        card.SetToFreeThisTurn();
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
}