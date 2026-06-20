using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Ancient;

public class HighTide: PercyJacksonCard
{
    public HighTide() : base(0, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy)
    {
        WithTide(2, 1);
        WithVar("Mult", 3);
        WithCalculatedDamage(0,
            (card, _) => card.DynamicVars["Mult"].BaseValue * card.Owner.PlayerCombatState.Tide().MaxTide);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        TideManager.UpdateMaxTide(Owner, DynamicVars["Tide"].IntValue);
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue);
        await CommonActions.CardAttack(this, play.Target, DynamicVars.CalculatedDamage, ValueProp.Move,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}