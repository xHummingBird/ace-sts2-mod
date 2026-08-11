using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Ace.AceCode.Mechanics;

public partial class CardDisplayOverlay : Control {
  public static CardDisplayOverlay? Instance { get; private set; }

  private Control? _cardDisplay;

  private TextureRect? _orb;

  private TextureRect[]? _slots;

  private RichTextLabel? _orbLabel;

  private Player? _player;

  private bool _exiting;

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
    _cardDisplay.Position = new Vector2(-135, -100);

    _orb = _cardDisplay.GetNodeOrNull<TextureRect>("Orb");

    //
    // The node names do not follow the layout, so the slots are ordered by
    // where they actually sit. Oldest card on the left.
    //
    _slots = new[] { "Card1", "Card2", "Card3", "Card4" }
                 .Select(name => _cardDisplay.GetNodeOrNull<TextureRect>(name))
                 .Where(rect => rect != null)
                 .OrderBy(rect => rect!.Position.X)
                 .ToArray()!;

    _orbLabel = _cardDisplay.GetNodeOrNull<RichTextLabel>("OrbLabel");

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

    _shown.Clear();
    _shown.AddRange(items);
    _shownValid = true;

    if (_slots != null) {
      for (int i = 0; i < _slots.Length; i++) {
        var slot = _slots[i];

        if (i < items.Count) {
          slot.Texture = CardTextures[items[i]];

          slot.Visible = true;
        } else {
          slot.Visible = false;
        }
      }
    }

    if (_orb != null) {
      _orb.Texture = GetOrbTexture();
    }

    if (_orbLabel != null) {
      _orbLabel.Text = $"[center]{items.Count}[/center]";
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

  public override void _ExitTree() {
    _exiting = true;

    _orb = null;

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
