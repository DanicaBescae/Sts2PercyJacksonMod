using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class DeadlyGamble: PercyJacksonCard
{
    public DeadlyGamble() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(3, 2);
        WithPower<IntangiblePower>(2);
        WithPower<PoisonPower>(5);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (var card in PileType.Hand.GetPile(Owner).Cards.ToList())
        {
            _ = await CardPileCmd.Add(card, PileType.Draw);
        }

        await CardPileCmd.Shuffle(choiceContext, Owner);
        await CommonActions.Draw(this, choiceContext);
        var drewNonAttack = false;
        foreach (var card in PileType.Hand.GetPile(Owner).Cards.ToList())
        {
            if (card.Type is CardType.Skill or CardType.Power) drewNonAttack = true;
        }

        if (drewNonAttack)
            await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature,
                DynamicVars["IntangiblePower"].BaseValue, Owner.Creature, this);
        else await PowerCmd.Apply<PoisonPower>(choiceContext, Owner.Creature,
            DynamicVars["PoisonPower"].BaseValue, Owner.Creature, this);
    }
}