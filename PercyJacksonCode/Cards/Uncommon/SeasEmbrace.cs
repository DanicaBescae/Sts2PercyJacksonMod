using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards.Token;

namespace PercyJackson.PercyJacksonCode.Cards.Uncommon;

public class SeasEmbrace : PercyJacksonCard
{
    public SeasEmbrace() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(10, 2);
        WithPower<VigorPower>(2);
        WithTip(typeof(Water));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue,
            Owner.Creature, this);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (var original in PileType.Hand.GetPile(Owner).Cards.ToList())
        {
            var card = CombatState.CreateCard<Water>(Owner);
            await CardCmd.Transform(original, card);
        }
    }
}