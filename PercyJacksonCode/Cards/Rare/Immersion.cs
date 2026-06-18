using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards.Token;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Immersion: PercyJacksonCard
{
    public Immersion() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithEnergy(3,1);
        WithTip(typeof(Water));
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var waterDraw = CombatState.CreateCard<Water>(Owner);
        await CardPileCmd.Add(waterDraw, PileType.Draw, CardPilePosition.Random);
        var waterHand = CombatState.CreateCard<Water>(Owner);
        await CardPileCmd.Add(waterHand, PileType.Hand);
        var waterDiscard = CombatState.CreateCard<Water>(Owner);
        await CardPileCmd.Add(waterDiscard, PileType.Discard);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}