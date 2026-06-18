using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class CategoryFive : PercyJacksonCard
{
    protected override bool IsPlayable => CombatManagerPlus.GetTimesShuffledThisCombat(Owner) >= DynamicVars["ShuffleCount"].BaseValue;
    
    public CategoryFive() : base(0, CardType.Attack,
        CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(30, 10);
        WithVar("ShuffleCount", 5);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
    }
}