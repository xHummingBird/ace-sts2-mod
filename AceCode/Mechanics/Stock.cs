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

  private sealed class StockState {
    public readonly List<AceColor> Items = [];
  }

  // Keyed on PlayerCombatState, which is rebuilt every combat, so the stock
  // clears itself without a reset hook.
  private static readonly NotNullSpireField<PlayerCombatState, StockState>
      Field = new(() => new StockState());

  private static List<AceColor>? Mutable(Player player) =>
      player.PlayerCombatState is {}
  combatState? Field[combatState].Items : null;

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

  // Stock is a stack, so Flip takes from the back.
  public static IReadOnlyList<AceColor> ConsumeLast(Player player, int amount) {
    if (Mutable(player) is not {} items)
      return [];

    var taken = new List<AceColor>();
    for (var i = 0; i < amount && items.Count > 0; i++) {
      taken.Add(items[^1]);
      items.RemoveAt(items.Count - 1);
    }

    return taken;
  }

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
