using Ace.AceCode.Cards.Ancient;
using Ace.AceCode.Cards.Flip;
using Ace.AceCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using Barrier = Ace.AceCode.Cards.Flip.Barrier;

namespace Ace.AceCode.Mechanics;

public partial class CardDisplayOverlay : Control {
  public static CardDisplayOverlay? Instance { get; private set; }

  private Control? _cardDisplay;

  private TextureRect? _orb;

  private TextureRect[]? _slots;

  private RichTextLabel? _orbLabel;
  
  private IHoverTip? _flipHoverTip;
  private IHoverTip? _stockHoverTip;

  private Player? _player;

  private bool _exiting;
  
  private Tween?[]? _slotTweens;
  
  private void UpdateCardPivots()
  {
    if (_slots == null)
      return;

    foreach (var slot in _slots)
      SetPivotToCenter(slot);
  }

  private static void SetPivotToCenter(TextureRect? slot)
  {
    if (slot == null)
      return;

    slot.PivotOffset = slot.Size / 2f;
  }
  
  private void OnHovered(
    Control? control,
    IHoverTip? hoverTip,
    Vector2 offset)
  {
    if (control == null ||
        hoverTip == null)
    {
      return;
    }

    NHoverTipSet.Clear();

    var tip =
      NHoverTipSet.CreateAndShow(
        control,
        hoverTip);

    if (tip != null)
    {
      tip.MouseFilter =
        MouseFilterEnum.Ignore;

      tip.GlobalPosition =
        control.GlobalPosition +
        offset;
    }
  }
  
  private void ShowStockTip()
  {
    OnHovered(
      _cardDisplay,
      _stockHoverTip,
      new Vector2(60f, -250f));
  }

  private void ShowFlipTip()
  {
    OnHovered(
      _orb,
      _flipHoverTip,
      new Vector2(20f, -450f));
  }

  private void OnUnhovered(
    Control? control)
  {
    if (control != null)
    {
      NHoverTipSet.Remove(control);
    }
  }

  private void PlayCardDisappearAnimation(
    TextureRect slot,
    ref Tween? tween)
  {
    tween?.Kill();

    slot.PivotOffset =
      slot.Size / 2f;

    slot.Scale =
      Vector2.One;

    slot.Modulate =
      new Color(
        1f,
        1f,
        1f,
        0.99f);

    tween =
      CreateTween();

    tween.Parallel()
      .TweenProperty(
        slot,
        "scale",
        new Vector2(1.3f, 1.3f),
        0.4f)
      .SetTrans(
        Tween.TransitionType.Back)
      .SetEase(
        Tween.EaseType.Out);

    tween.Parallel()
      .TweenProperty(
        slot,
        "modulate:a",
        0f,
        0.4f);

    tween.TweenCallback(
      Callable.From(() =>
      {
        slot.Visible = false;

        slot.Scale =
          Vector2.One;

        slot.Modulate =
          Colors.White;
      }));
  }

  private static readonly Dictionary<AceColor, Texture2D?> CardTextures =
      new() {
        [AceColor.Red] = LoadCardUi("card_red"),
        [AceColor.Blue] = LoadCardUi("card_blue"),
        [AceColor.Yellow] = LoadCardUi("card_yellow"),
        [AceColor.White] = LoadCardUi("card_white"),
      };

  private static readonly Dictionary<AceColor, Texture2D?> OrbTextures = new() {
    [AceColor.Red] = LoadCardUi("orb_red"),
    [AceColor.Blue] = LoadCardUi("orb_blue"),
    [AceColor.Yellow] = LoadCardUi("orb_yellow"),
    [AceColor.White] = LoadCardUi("orb_white"),
  };

  private static readonly Texture2D? OrbNoneTexture = LoadCardUi("orb_none");
  private static readonly Texture2D? OrbAllTexture = LoadCardUi("orb_all");

  //
  // Cached so the textures are only reassigned when the stock changes
  //
  private readonly List<AceColor> _shown = [];
  private bool _shownValid;

  private static Texture2D? LoadCardUi(string name) =>
      GD.Load<Texture2D>($"res://Ace/images/card_ui/{name}.png");

  public override void _Ready() {
    Instance = this;
    Name = "CardDisplayOverlay";

    MouseFilter = MouseFilterEnum.Pass;

    CallDeferred(nameof(Setup));
  }
  
  private async void Setup() {
    if (!IsInsideTree())
      return;

    for (int i = 0; i < 60; i++) {
      if (_exiting || !IsInsideTree())
        return;

      var state = CombatManager.Instance?.DebugOnlyGetState();

      var player = state?.Players.FirstOrDefault(p => LocalContext.IsMe(p));

      if (player != null) {
        if (player.Character is not Character.Ace) {
          QueueFree();
          return;
        }

        _player = player;
        break;
      }

      await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    if (_player == null) {
      QueueFree();
      return;
    }

    var scene =
        GD.Load<PackedScene>("res://Ace/scenes/card_holder_display.tscn");

    if (scene == null) {
      GD.PushError("[Ace] Failed to load card_holder_display.tscn");

      QueueFree();
      return;
    }

    _cardDisplay = scene.Instantiate<Control>();

    AddChild(_cardDisplay);

    _cardDisplay.MouseFilter = MouseFilterEnum.Pass;

    _cardDisplay.SetAnchorsPreset(LayoutPreset.BottomLeft);

    //
    // Tweak as needed
    //
    _cardDisplay.Position = new Vector2(-135, -130);

    _orb = _cardDisplay.GetNodeOrNull<TextureRect>("Orb");
    
    _stockHoverTip =
      AceStaticHoverTip.Stock;

    _cardDisplay.MouseFilter =
      MouseFilterEnum.Pass;

    _cardDisplay.MouseEntered += ShowStockTip;

    _cardDisplay.MouseExited +=
      () => OnUnhovered(
        _cardDisplay);

    //
    // The node names do not follow the layout, so the slots are ordered by
    // where they actually sit. Oldest card on the left.
    //
    _slots = new[] { "Card1", "Card2", "Card3", "Card4" }
                 .Select(name => _cardDisplay.GetNodeOrNull<TextureRect>(name))
                 .Where(rect => rect != null)
                 .OrderBy(rect => rect!.Position.X)
                 .ToArray()!;
    
    _slotTweens = new Tween?[_slots.Length];

    CallDeferred(nameof(UpdateCardPivots));

    _orbLabel = _cardDisplay.GetNodeOrNull<RichTextLabel>("OrbLabel");
    
    if (_orb != null)
    {
      _orb.MouseFilter =
        MouseFilterEnum.Pass;

      _orb.MouseEntered += ShowFlipTip;

      _orb.MouseExited +=
        () => OnUnhovered(
          _orb);
    }

    if (_orbLabel != null) {
      SetupLabel(_orbLabel);
      _orbLabel.Position += new Vector2(0, 17);
    }

    RefreshDisplay();
  }

  public override void _Process(double delta) {
    if (_exiting)
      return;

    if (_player == null)
      return;

    if (!CombatManager.Instance.IsInProgress)
      return;

    RefreshDisplay();
  }
  
  private void PlayCardAppearAnimation(
    TextureRect slot,
    ref Tween? tween)
  {
    tween?.Kill();
    tween = null;

    slot.PivotOffset =
      slot.Size / 2f;

    slot.Visible = true;

    slot.Scale =
      new Vector2(
        1.3f,
        1.3f);

    slot.Modulate =
      Colors.White;

    tween =
      CreateTween();

    tween.TweenProperty(
        slot,
        "scale",
        Vector2.One,
        0.30f)
      .SetTrans(
        Tween.TransitionType.Back)
      .SetEase(
        Tween.EaseType.Out);
  }

  private void SetupLabel(RichTextLabel label) {
    var font = GD.Load<Font>("res://themes/kreon_bold_shared.tres");

    if (font != null) {
      label.AddThemeFontOverride("font", font);

      label.AddThemeFontOverride("normal_font", font);
    }

    label.BbcodeEnabled = true;

    label.AddThemeColorOverride("default_color", Colors.White);

    label.AddThemeColorOverride("font_outline_color", Colors.Black);

    label.AddThemeConstantOverride("outline_size", 8);

    label.AddThemeFontSizeOverride("normal_font_size", 32);

    label.MouseFilter = MouseFilterEnum.Ignore;
  }

  private void RefreshDisplay() {
    if (_player == null)
      return;

    var items = Stock.Items(_player);

    if (_shownValid && _shown.SequenceEqual(items))
      return;
    
    var previousCount = _shown.Count;

    _shown.Clear();
    _shown.AddRange(items);
    _shownValid = true;
    
    RefreshFlipHoverTip();
    
    var newCount = items.Count;

    if (_slots != null)
    {
      _slotTweens ??= new Tween?[_slots.Length];

      for (int i = 0; i < _slots.Length; i++)
      {
        var slot =
          _slots[i];

        bool hasCard =
          i < items.Count;

        if (hasCard)
        {
          slot.Texture =
            CardTextures[items[i]];

          bool cardJustAdded =
            i >= previousCount;

          if (cardJustAdded)
          {
            PlayCardAppearAnimation(
              slot,
              ref _slotTweens[i]);
          }
          else
          {
            _slotTweens[i]?.Kill();
            _slotTweens[i] = null;

            slot.Visible = true;
            slot.Scale = Vector2.One;
            slot.Modulate = Colors.White;
          }
        }
        else
        {
          if (slot.Visible &&
              slot.Modulate.A > 0.01f)
          {
            PlayCardDisappearAnimation(
              slot,
              ref _slotTweens[i]);
          }
        }
      }
    }

    if (_orb != null) {
      _orb.Texture = GetOrbTexture();
    }

    if (_orbLabel != null) {
      // Rainbow stock (all 4 colors present)
      if (Stock.IsRainbow(_player)) {
        _orbLabel.Text = "";
      } else {
        var majority = Stock.Majority(_player);

        if (majority is {} color) {
          var count = items.Count(x => x == color);
          _orbLabel.Text = $"[center]{count}[/center]";
        } else {
          _orbLabel.Text = "";
        }
      }
    }
  }

  private Texture2D? GetOrbTexture() {
    if (_player == null)
      return OrbNoneTexture;

    if (Stock.Count(_player) == 0)
      return OrbNoneTexture;

    if (Stock.IsRainbow(_player))
      return OrbAllTexture;

    return Stock.Majority(_player) is {}
    color ? OrbTextures[color] : OrbNoneTexture;
  }
  
  private void RefreshFlipHoverTip()
  {
    if (_player == null)
    {
      _flipHoverTip = null;
      return;
    }

    var card = Flip.Preview(_player);

    _flipHoverTip =
      card != null
        ? HoverTipFactory.FromCard(card)
        : null;
  }

  public override void _ExitTree() {
    _exiting = true;
    
    NHoverTipSet.Remove(_orb);
    NHoverTipSet.Remove(_cardDisplay);

    _flipHoverTip = null;
    _stockHoverTip = null;

    _orb = null;
    
    if (_slotTweens != null)
    {
      foreach (var tween in _slotTweens)
        tween?.Kill();

      _slotTweens = null;
    }

    _slots = null;

    _orbLabel = null;

    _cardDisplay = null;
    _player = null;

    if (Instance == this)
      Instance = null;
    

  }
}

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter._Ready))]
public static class CardDisplayOverlayPatch {
  public static void Postfix(NEnergyCounter __instance) {
    if (__instance == null)
      return;

    if (!GodotObject.IsInstanceValid(__instance))
      return;

    if (__instance.IsQueuedForDeletion())
      return;

    if (__instance.GetNodeOrNull<CardDisplayOverlay>("CardDisplayOverlay") !=
        null)
      return;

    var overlay = new CardDisplayOverlay { Name = "CardDisplayOverlay" };

    __instance.AddChild(overlay);
  }
  
}


