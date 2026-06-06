using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Basic;

public class IncomingWave: PercyJacksonCard
{
    public IncomingWave() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithTide(1, 1);
        WithCalculatedVar("CalculatedNewDamage", 1,
            (c, _) => TideManager.GetNewTide(c.Owner, c.DynamicVars["Tide"].IntValue));
        WithCalculatedDamage(1,
            (card, _) => card.Owner.PlayerCombatState.Tide().CurrentTide);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue);
        await CommonActions.CardAttack(this, play.Target, calculatedDamage: DynamicVars.CalculatedDamage,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}