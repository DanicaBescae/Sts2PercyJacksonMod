using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards.Ancient;
using PercyJackson.PercyJacksonCode.Extensions;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Models;

namespace PercyJackson.PercyJacksonCode.Cards.Basic;

public class IncomingWave: PercyJacksonCard, ITranscendenceCard
{
    public IncomingWave() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithTide(1, 1);
        WithCalculatedDamage(1,
            (c, _) => TideManager.GetNewTide(c.Owner, c.DynamicVars["Tide"].IntValue, out var _));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play.Target, calculatedDamage: DynamicVars.CalculatedDamage,
            vfx: "vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        await TideManager.UpdateTide(Owner, DynamicVars["Tide"].IntValue);
    }
    
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<HighTide>();
    }
}