using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class Whirlpool: PercyJacksonCard
{
    public Whirlpool() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(5, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var tideLost = Owner.PlayerCombatState.Tide().CurrentTide;
        await TideManager.UpdateTide(Owner, Owner.PlayerCombatState.Tide().CurrentTide, true);
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, tideLost,
                vfx: "vfx/vfx_giant_horizontal_slash",
                tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }
}