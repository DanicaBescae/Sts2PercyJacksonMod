using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Hooks;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Powers;

public class DeepSeaTreasurePower() : PercyJacksonPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side) return Task.CompletedTask;
        foreach (var card in PileType.Discard.GetPile(Owner.Player).Cards
                     .Where(c => c.EnergyCost.GetAmountToSpend() > 0 && !c.EnergyCost.CostsX).ToList()
                     .UnstableShuffle(Owner.Player.RunState.Rng.CombatCardSelection)
                     .Take(Amount))
        {
            card.EnergyCost.SetUntilPlayed(0);
            CardCmd.Preview(card);
        }

        return Task.CompletedTask;
    }
}