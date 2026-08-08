using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Ace.AceCode.Mechanics;

public enum AceColor { Red, Blue, Yellow, White }

// Cards define what stock color they represent
// TODO: (low priority) have stock color auto detected rather than a card
// explicitly defining the color
public interface IStockingCard {
  AceColor StockColor { get; }
}

public static class Stock {
  public const int MaxSlots = 4;

  // Keyed on Player so the stock rides along in SerializablePlayer, which is
  // part of the multiplayer rejoin payload. Player lives for the whole run, so
  // AceStockModel has to clear this at the start of every combat.
  private static readonly SavedSpireField<Player, List<AceColor>> Field =
      new(() => [], "stock") {
        Serializer =
            (items, writer) => {
              writer.WriteInt(items.Count);
              foreach (var item in items)
                writer.WriteInt((int)item);
            },
        Deserializer =
            reader => {
              var count = reader.ReadInt();
              var items = new List<AceColor>(count);
              for (var i = 0; i < count; i++)
                items.Add((AceColor)reader.ReadInt());
              return items;
            },
      };

  // Without this the class is beforefieldinit and the runtime is free to skip
  // the field setup when Register is called.
  static Stock() {}

  // The SavedSpireField registers itself when the field above is built, so
  // something has to touch Stock before BaseLib collects the fields.
  public static void Register() {}

  internal static List<AceColor>? Mutable(Player? player) =>
      player is null ? null : Field[player];

  private static readonly AceColor[] Empty = [];

  public static IReadOnlyList<AceColor>
  Items(Player player) => Mutable(player) ?? (IReadOnlyList<AceColor>)Empty;

  public static int Count(Player player) => Items(player).Count;

  public static int Count(Player player,
                          AceColor color) => Items(player).Count(item => item ==
                                                                         color);

  public static AceColor? Top(Player player) {
    var items = Items(player);
    return items.Count > 0 ? items[^1] : null;
  }

  // can be called from anywhere to add a card to the stock queue
  // currently hooked up in AceStockModel.cs for every stockablecard playued
  public static void Push(Player player, AceColor color) {
    if (Mutable(player) is not {} items)
      return;

    // Full stock drops the oldest card to make room.
    if (items.Count >= MaxSlots)
      items.RemoveAt(0);

    items.Add(color);
  }

  public static void Clear(Player player) => Mutable(player)?.Clear();

  // Every card has a different color, only counts once the stock is full.
  public static bool IsRainbow(Player player) {
    var items = Items(player);
    return items.Count == MaxSlots && items.Distinct().Count() == MaxSlots;
  }

  // The color holding the most slots. Ties go to whichever color reached that
  // amount first, so the older card wins.
  public static AceColor? Majority(Player player) {
    var items = Items(player);
    if (items.Count == 0)
      return null;

    var most = items.GroupBy(item => item).Max(group => group.Count());

    AceColor? best = null;
    var bestIndex = int.MaxValue;

    foreach (var color in items.Distinct()) {
      var seen = 0;
      for (var i = 0; i < items.Count; i++) {
        if (items[i] != color)
          continue;

        if (++seen < most)
          continue;

        if (i < bestIndex) {
          bestIndex = i;
          best = color;
        }
        break;
      }
    }

    return best;
  }
}
