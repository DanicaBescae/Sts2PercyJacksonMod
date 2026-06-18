using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class WaveAfterWave : PercyJacksonCard
{
    public WaveAfterWave() : base(1, CardType.Attack,
        CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(3);
        WithVar("HitCount", 3, 1);
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars["HitCount"].IntValue).Execute(choiceContext);
        for (var i = 0; i < DynamicVars["HitCount"].IntValue; i++)
        {
            var water = CombatState.CreateCard<Water>(Owner);
            await CardPileCmd.Add(water, PileType.Hand);
        }
    }
}