using Ace.AceCode.Relics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Ace.AceCode.Mechanics;

// Consume and Flip are plain statics with no hook of their own, so the relics reacting to them are
// read from here instead of from a model hook.
public static class AceRelicTriggers
{
    public static void OnConsumed(Player? player, IReadOnlyList<AceColor> taken)
    {
        if (player is null || taken.Count == 0)
            return;

        if (player.GetRelic<ArcaneDeck>() is not null)
            Stock.Push(player, AceColor.White);

        if (player.GetRelic<AkedemeiaDeck>() is not null && taken.Contains(AceColor.Red))
            Stock.Push(player, AceColor.Red);
    }

    public static int FlipLevelBonus(Player? player, AceColor? color)
    {
        if (player is null)
            return 0;

        var bonus = 0;

        if (player.GetRelic<BlackTrump>() is not null)
            bonus++;

        if (color == AceColor.Blue && player.GetRelic<MythrilDeck>() is not null)
            bonus++;

        return bonus;
    }

    // Kept separate from the flip path so a Consume card (Ace in the Hole) can pay out Crazy Eights
    // without picking up the flip level bonuses.
    public static async Task OnSpectrumPayoff(Player? player)
    {
        if (player is null || player.GetRelic<CrazyEights>() is null)
            return;

        await PlayerCmd.GainEnergy(1m, player);
    }
}
