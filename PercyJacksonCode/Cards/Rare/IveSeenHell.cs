using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using PercyJackson.PercyJacksonCode.Cards;

namespace PercyJackson.PercyJacksonCode.Cards.Rare;

public class IveSeenHell : PercyJacksonCard
{
    public IveSeenHell() : base(2, CardType.Skill,
        CardRarity.Rare, TargetType.Self)
    {
        WithVar("AdditionalBlock", 2);
        WithCalculatedBlock(7,
            (card, _) => card.Owner.Creature.Powers
                             .Where(p => p is { Type: PowerType.Buff, StackType: PowerStackType.Counter })
                             .Sum(buff => buff.Amount) * card.DynamicVars["AdditionalBlock"].BaseValue,
            ValueProp.Move, 3);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
    }
}