using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class WavePunch: PercyJacksonCard
{
    public WavePunch() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVar("TideMult", 3, 1);
        WithCalculatedDamage(12, (card, _) => card.Owner.PlayerCombatState.Tide().TideGainedThisCombat * card.DynamicVars["TideMult"].IntValue);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, DynamicVars.CalculatedDamage, ValueProp.Move,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}