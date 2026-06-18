using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using PercyJackson.PercyJacksonCode.Fields;
using PercyJackson.PercyJacksonCode.Powers;

namespace PercyJackson.PercyJacksonCode.Extensions;

public static class PlayerCombatStateExtensions
{
    public class TideCombatState
    {
        public TideCombatState(PlayerCombatState playerCombatState)
        {
            Owner = playerCombatState._player;
        }
        
        public int CurrentTide         {
            get;
            set
            {
                if (field == value) return;
                var tide = field;
                var tideChange = value - field;
                if (Owner.HasPower<SonOfNeptunePower>())
                {
                    tideChange *= Owner.Creature.GetPowerAmount<SonOfNeptunePower>();
                }
                field = Math.Max(0, field + tideChange);
                TideChanged?.Invoke(tide, field);
            }
        } = 0;
        
        public int MaxTide         {
            get => TempMaxTide > 0 ? TempMaxTide : field;
            set
            {
                if (field == value) return;
                var maxTide = field;
                var tideChange = value - field;
                if (Owner.HasPower<SonOfNeptunePower>())
                {
                    tideChange *= Owner.Creature.GetPowerAmount<SonOfNeptunePower>();
                }
                field = Math.Max(0, field + tideChange);
                MaxTideChanged?.Invoke(maxTide, field);
            }
        } = 1;
        
        public int TempMaxTide         {
            get;
            set
            {
                if (field == value) return;
                var maxTide = field;
                var tideChange = value - field;
                if (Owner.HasPower<SonOfNeptunePower>())
                {
                    tideChange *= Owner.Creature.GetPowerAmount<SonOfNeptunePower>();
                }
                field = Math.Max(0, field + tideChange);
                MaxTideChanged?.Invoke(maxTide, field);
            }
        } = 0;

        private Player Owner
        {
            get;
            init
            {
                if (field != null && value != null && value != field)
                    throw new InvalidOperationException("Tide already has an owner.");

                field = value;
            }
        }

        public int TideGainedThisCombat;

        public event Action<int, int>? TideChanged;
        public event Action<int, int>? MaxTideChanged;
    }
    
    public class ComboCombatState
    {
        public ComboCombatState(PlayerCombatState playerCombatState)
        {
            Owner = playerCombatState._player;
        }
        public int CurrentComboCount
        {
            get;
            private set
            {
                if (field == value) return;
                var combo = field;
                field = value;
                ComboChanged?.Invoke(combo, field);
            }
        }

        public List<CardPlay> ComboHistory = [];

        public Player Owner
        {
            get;
            private set
            {
                if (field != null && value != null && value != field)
                    throw new InvalidOperationException("Tide already has an owner.");

                field = value;
            }
        }

        public void AddToCombo(CardPlay cardPlay)
        {
            ComboHistory.Add(cardPlay);
            CurrentComboCount = ComboHistory.Count;
            NewCardInCombo?.Invoke(cardPlay);
        }
        
        public void ClearCombo()
        {
            ComboHistory.Clear();
            CurrentComboCount = 0;
        }
        
        public event Action<int, int>? ComboChanged;
        public event Action<CardPlay>? NewCardInCombo;
    }
    
    public static TideCombatState Tide(this PlayerCombatState playerCombatState)
    {
        return PercyJacksonFields.TideCombatState[playerCombatState];
    }
    
    public static ComboCombatState Combo(this PlayerCombatState playerCombatState)
    {
        return PercyJacksonFields.ComboCombatState[playerCombatState];
    }
}