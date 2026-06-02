using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Fields;

namespace PercyJackson.PercyJacksonCode.Models;

public class TideCombatState: AbstractModel, ICustomModel
{
    public override bool ShouldReceiveCombatHooks { get; } = true;

    private int _currentTide;
    
    public decimal CurrentTide => _currentTide;

    private Player? _player;
    
    public Player Owner
    {
        get
        {
            AssertMutable();
            return _player;
        }
        set
        {
            AssertMutable();
            if (_player != null && value != null && value != _player)
                throw new InvalidOperationException("Rune " + Id.Entry + " already has an owner.");

            _player = value;
        }
    }

    public override Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return Task.CompletedTask;
        return ApplyVigorFromTide(choiceContext, Owner.Creature.CombatState, Owner.Creature, _currentTide);
    }
    
    private static async Task ApplyVigorFromTide(PlayerChoiceContext choiceContext, ICombatState? combatState, Creature player, int tide)
    {
        if (tide == 0) return;
        await PowerCmd.Apply<VigorPower>(choiceContext, player, tide, null, null);
    }

    public void UpdateTide(int tideChange)
    {
        if(tideChange >= 0) RaiseTide(tideChange);
        else LowerTide(tideChange);
    }

    private void RaiseTide(int tideChange)
    {
        _currentTide += tideChange;
    }

    private void LowerTide(int tideChange)
    {
        _currentTide += tideChange;
    }
}