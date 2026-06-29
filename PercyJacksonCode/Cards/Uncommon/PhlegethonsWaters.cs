using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class PhlegethonsWaters : PercyJacksonCard
{
    public PhlegethonsWaters() : base(1, CardType.Skill,
        CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(typeof(Burn));
        WithCards(2);
        WithPower<DexterityPower>(1, 1);
        WithPower<StrengthPower>(1, 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var burns = new CardPileAddResult[DynamicVars.Cards.IntValue];
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            var card = CombatState.CreateCard<Burn>(Owner);
            burns[i] = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, null, CardPilePosition.Random);
        }
        CardCmd.PreviewCardPileAdd(burns);
        await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars.Dexterity.BaseValue,
            Owner.Creature, this);
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue,
            Owner.Creature, this);
    }
}