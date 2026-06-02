using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Fields;

namespace PercyJackson.PercyJacksonCode.Cards.Basic;

public class IncomingWave: PercyJacksonCard
{
    public IncomingWave() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithCalculatedDamage(1,
            (card, _) => card.Owner.PlayerCombatState.Tide().CurrentTide);
        WithTide(1, 2);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        
        Owner.PlayerCombatState.Tide().UpdateTide(DynamicVars["Tide"].IntValue);
        await CommonActions.CardAttack(this, play.Target, calculatedDamage: DynamicVars.CalculatedDamage,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        await CommonActions.CardBlock(this, play);
    }
}