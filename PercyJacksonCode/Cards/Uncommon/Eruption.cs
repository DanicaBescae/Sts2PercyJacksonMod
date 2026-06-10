using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class Eruption : PercyJacksonCard
{
    public Eruption() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithTide(4);
        WithDamage(30, 6);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue, true);
        await CommonActions.CardAttack(this, play.Target, DynamicVars.Damage.BaseValue, ValueProp.Move,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }
}