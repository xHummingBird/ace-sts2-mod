using MegaCrit.Sts2.Core.Entities.Players;

namespace Ace.AceCode.Mechanics;

// Raw stock consumption. Every method gives back the colors it took out, so a
// card can decide what to do with them. Flip builds on top of this with the
// standard effects, cards that want their own effects use these directly.
public static class Consume {
  public static IReadOnlyList<AceColor> Majority(
    Player player,
    int amount)
  {
    return Stock.Majority(player) is {} color
      ? 
      OfColor(player, color, amount)
      : [];
  }
  
  public static IReadOnlyList<AceColor> Majority(Player player) =>
    Stock.Majority(player) is {} color
      ? OfColor(player, color)
      : [];
  
  public static IReadOnlyList<AceColor> OfColor(
    Player player,
    AceColor color)
  {
    if (Stock.Mutable(player) is not {} items)
      return [];

    var taken = items.Where(item => item == color).ToList();

    items.RemoveAll(item => item == color);

    return taken;
  }

  public static IReadOnlyList<AceColor> OfColor(
    Player player,
    AceColor color,
    int amount)
  {
    if (Stock.Mutable(player) is not {} items)
      return [];

    var taken = new List<AceColor>();

    for (var i = 0;
         i < items.Count && taken.Count < amount;)
    {
      if (items[i] == color)
      {
        taken.Add(items[i]);
        items.RemoveAt(i);
        continue;
      }

      i++;
    }

    return taken;
  }

  public static IReadOnlyList<AceColor> All(Player player) {
    if (Stock.Mutable(player) is not {} items)
      return [];

    var taken = items.ToList();
    items.Clear();
    return taken;
  }

  // Stock is a stack, so this takes the newest cards first.
  public static IReadOnlyList<AceColor> Last(Player player, int amount) {
    if (Stock.Mutable(player) is not {} items)
      return [];

    var taken = new List<AceColor>();
    for (var i = 0; i < amount && items.Count > 0; i++) {
      taken.Add(items[^1]);
      items.RemoveAt(items.Count - 1);
    }

    return taken;
  }

  public static IReadOnlyList<AceColor> First(Player player, int amount) {
    if (Stock.Mutable(player) is not {} items)
      return [];

    var taken = new List<AceColor>();
    for (var i = 0; i < amount && items.Count > 0; i++) {
      taken.Add(items[0]);
      items.RemoveAt(0);
    }

    return taken;
  }
}
