using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using PercyJackson.PercyJacksonCode.Extensions;

namespace PercyJackson.PercyJacksonCode.Nodes;

public partial class NTideCounter: Control
{
	private static readonly StringName _v = new("v");
	private static readonly StringName _s = new("s");

	private static readonly (Color, Color, Color) BlueFontColor = (new Color("e3e3ff"), new Color("00000030"),
		new Color("111154"));
	
	private Player? _player;
	
	private float _velocity1;
	private float _velocity2;
	
	private decimal _displayedTideCount = -1;
	private int _displayedMaxTideCount = -1;
	private MegaLabel? _label;
	private float _lerpingTideCount;
	private float _lerpingMaxTideCount;
	private TextureRect _icon = null!;
	private ShaderMaterial _hsv = null!;
	private Tween? _hsvTween;
	
	private bool _isListeningToCombatState;
	
	private HoverTip _hoverTip;
	
	public void Initialize(Player player)
	{
		_player = player;
		ConnectTideChangedSignal();
		RefreshVisibility();
	}

	public override void _Ready()
	{
		_icon = GetNode<TextureRect>("%Icon");
		_hsv = (ShaderMaterial)_icon.Material;
		
		_label = CreateLabel(BlueFontColor);
		GetNode<MarginContainer>("%TextContainer").AddChild(_label);
		
		var locString = new LocString("card_keywords", "PERCYJACKSON-TIDEKEYWORD.description");
		locString.Add("tide", 0);
		_hoverTip = new HoverTip(new LocString("card_keywords", "PERCYJACKSON-TIDEKEYWORD.title"), locString);
		
		Connect(Control.SignalName.MouseEntered, Callable.From(OnHovered));
		Connect(Control.SignalName.MouseExited, Callable.From(OnUnhovered));
		
		SetTideCountText(0, GetPlayerMaxTide(_player), true);
		Visible = false;
	}

	private static MegaLabel CreateLabel((Color, Color, Color) fontColor)
	{
		var label = new MegaLabel();
		label.MaxFontSize = 28;
		label.AutoSizeEnabled = false;
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.AddThemeColorOverride("font_color", fontColor.Item1);
		label.AddThemeColorOverride("font_shadow_color", fontColor.Item2);
		label.AddThemeColorOverride("font_outline_color", fontColor.Item3);
		label.AddThemeConstantOverride("shadow_offset_x", 3);
		label.AddThemeConstantOverride("shadow_offset_y", 3);
		label.AddThemeConstantOverride("outline_size", 15);
		label.AddThemeConstantOverride("shadow_outline_size", 15);
		label.AddThemeFontSizeOverride("font_size", 28);
		label.Text = "0/0";

		return label;
	}
	
	public override void _EnterTree()
	{
		base._EnterTree();
		ConnectTideChangedSignal();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (_player == null || !_isListeningToCombatState) return;
		
		var tideCombatState = _player.PlayerCombatState?.Tide();
		tideCombatState?.TideChanged -= OnTideChanged;
		tideCombatState?.MaxTideChanged -= OnMaxTideChanged;
		_isListeningToCombatState = false;
	}
	
	private void ConnectTideChangedSignal()
	{
		if (_player == null || _isListeningToCombatState) return;
		
		var tideCombatState = _player.PlayerCombatState?.Tide();
		tideCombatState?.TideChanged += OnTideChanged;
		tideCombatState?.MaxTideChanged += OnMaxTideChanged;
		_isListeningToCombatState = true;
	}
	
	private void OnHovered()
	{
		var nHoverTipSet = NHoverTipSet.CreateAndShow(this, _hoverTip);
		nHoverTipSet?.GlobalPosition = GlobalPosition + new Vector2(-70, -240);
	}
	
	private void OnUnhovered()
	{
		NHoverTipSet.Remove(this);
	}
	
	private void OnTideChanged(int oldTide, int newTide)
	{
		UpdateTideCount(oldTide, newTide);
		RefreshVisibility();
	}
	
	private void OnMaxTideChanged(int oldMax, int newMax)
	{
		UpdateMaxTide(oldMax, newMax);
		RefreshVisibility();
	}
	
	public override void _Process(double delta)
	{
		if (_player == null) return;
		var tide = GetPlayerTide(_player);
		var maxTide = GetPlayerMaxTide(_player);

		_lerpingTideCount =
			MathHelper.SmoothDamp(_lerpingTideCount, tide, ref _velocity1, 0.5f, (float)delta);
		_lerpingMaxTideCount = MathHelper.SmoothDamp(_lerpingMaxTideCount, maxTide, ref _velocity2, 0.5f, (float)delta);
		SetTideCountText((int)MathF.Round(_lerpingTideCount), (int)MathF.Round(_lerpingMaxTideCount));
	}
	
	private static int GetPlayerTide(Player player)
	{
		var tide = player.PlayerCombatState?.Tide().CurrentTide ?? 0;
		return tide;
	}
	
	private static int GetPlayerMaxTide(Player player)
	{
		var tide = player.PlayerCombatState?.Tide().MaxTide ?? 0;
		return tide;
	}
	
	private void UpdateTideCount(int oldCount, int newCount)
	{
		if (newCount < oldCount)
		{
			_hsvTween?.Kill();
			_hsv.SetShaderParameter(_v, 1f);
			_lerpingTideCount = newCount;
			SetTideCountText(newCount, GetPlayerMaxTide(_player));
		}
		else if (newCount > oldCount)
		{
			_hsvTween?.Kill();
			_hsvTween = CreateTween();
			_hsvTween.TweenMethod(Callable.From<float>(UpdateShaderV), 2f, 1f, 0.2);
			//TODO vfx gain tide
		}
	}
	
	private void UpdateMaxTide(int oldMax, int newMax)
	{
		if (newMax < oldMax)
		{
			_hsvTween?.Kill();
			_hsv.SetShaderParameter(_v, 1f);
			_lerpingMaxTideCount = newMax;
			SetTideCountText(GetPlayerTide(_player), newMax);
		}
		else if (newMax > oldMax)
		{
			_hsvTween?.Kill();
			_hsvTween = CreateTween();
			_hsvTween.TweenMethod(Callable.From<float>(UpdateShaderV), 2f, 1f, 0.2);
		}
	}
	
	private void SetTideCountText(int tide, int maxTide, bool initSetup = false)
	{
		if (!initSetup && _displayedTideCount == tide && _displayedMaxTideCount == maxTide) return;
		
		_displayedTideCount = tide;
		_displayedMaxTideCount = maxTide;
		var label = _label;
		var fontColor = BlueFontColor.Item1;

		label.AddThemeColorOverride(ThemeConstants.Label.FontColor, tide == 0 ? StsColors.red : fontColor);
		label.SetTextAutoSize(tide + "/" + maxTide);

		if (tide == 0)
		{
			_hsv.SetShaderParameter(_s, 0.5f);
			_hsv.SetShaderParameter(_v, 0.85f);
		}
		else
		{
			_hsv.SetShaderParameter(_s, 1f);
			_hsv.SetShaderParameter(_v, 1f);
		}
	}

	private void UpdateShaderV(float value)
	{
		_hsv.SetShaderParameter(_v, value);
	}
	
	private void RefreshVisibility()
	{
		if (_player == null)
		{
			Visible = false;
			return;
		}

		var shouldAlwaysShowTide = _player.Character is Character.PercyJackson;

		Visible = Visible || shouldAlwaysShowTide || GetPlayerMaxTide(_player) > 0;
	}
}
