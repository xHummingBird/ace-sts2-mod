using Ace.AceCode.Cards.Ancient;
using Ace.AceCode.Cards.Flip;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Barrier = Ace.AceCode.Cards.Flip.Barrier;

namespace Ace.AceCode.Mechanics;

public readonly record struct FlipResult(
    AceColor? Color,
    int Consumed,
    int Level,
    bool IsSpectrum = false)
{
    public bool Happened => Consumed > 0;
}

public static class Flip
{
    public const int MaxColorLevel = 5;
    public const int MaxSpectrumLevel = 2;

    public static Task<FlipResult> Majority(
        PlayerChoiceContext choiceContext,
        CardModel card,
        CardPlay play,
        int levelBonus = 0,
        int? maxCount = null)
    {
        var color = Stock.Majority(card.Owner);

        var consumed =
            maxCount is { } cap
                ? Consume.Majority(card.Owner, cap)
                : Consume.Majority(card.Owner);

        return ResolveColor(
            choiceContext,
            card,
            play,
            color,
            consumed,
            levelBonus);
    }
    
    public static Task<FlipResult> Oldest(
        PlayerChoiceContext choiceContext,
        CardModel card,
        CardPlay play,
        int levelBonus = 0)
    {
        var oldest = Stock.Items(card.Owner).FirstOrDefault();

        if (Stock.Count(card.Owner) == 0)
            return Task.FromResult(default(FlipResult));

        return ResolveColor(
            choiceContext,
            card,
            play,
            oldest,
            Consume.First(card.Owner, 1),
            levelBonus);
    }

    public static Task<FlipResult> Color(
        PlayerChoiceContext choiceContext,
        CardModel card,
        CardPlay play,
        AceColor color,
        int levelBonus = 0,
        int? maxCount = null)
    {
        var consumed =
            maxCount is { } cap
                ? Consume.OfColor(card.Owner, color, cap)
                : Consume.OfColor(card.Owner, color);

        return ResolveColor(
            choiceContext,
            card,
            play,
            color,
            consumed,
            levelBonus);
    }

    // Consume all stock, but use the majority color to decide which Flip card is played.
    public static Task<FlipResult> All(
        PlayerChoiceContext choiceContext,
        CardModel card,
        CardPlay play,
        int levelBonus = 0)
    {
        var color = Stock.Majority(card.Owner);

        return ResolveColor(
            choiceContext,
            card,
            play,
            color,
            Consume.All(card.Owner),
            levelBonus);
    }

    // Use this for Spectrum / Royal Flush style cards.
    // Normal mode:
    //   Only works if the current stock is Spectrum: all 4 colors are different.
    //
    // Force mode:
    //   Consumes all stock and plays a Spectrum card even if the stock was not actually Spectrum.
    //   Useful for Ancient cards like "Consume all Stock. Perform a Spectrum Flip."
    public static Task<FlipResult> Spectrum(
        PlayerChoiceContext choiceContext,
        CardModel card,
        CardPlay play,
        int levelBonus = 0,
        bool force = false)
    {
        var isSpectrum = Stock.IsRainbow(card.Owner);

        if (!isSpectrum && !force)
            return Task.FromResult(default(FlipResult));

        return ResolveSpectrum(
            choiceContext,
            card,
            play,
            Consume.All(card.Owner),
            levelBonus);
    }

    private static async Task<FlipResult> ResolveColor(
        PlayerChoiceContext choiceContext,
        CardModel sourceCard,
        CardPlay sourcePlay,
        AceColor? color,
        IReadOnlyList<AceColor> consumed,
        int levelBonus)
    {
        if (color is not { } flipped || consumed.Count == 0)
            return default;

        var level = Math.Clamp(
            consumed.Count + levelBonus,
            1,
            MaxColorLevel);

        var flipModel = GetColorFlipCard(
            flipped,
            level);

        if (flipModel is null)
            return new FlipResult(
                flipped,
                consumed.Count,
                level);

        var flipCard =
            sourceCard.CombatState.CreateCard(
                flipModel,
                sourceCard.Owner);

        await AutoPlayFlipCard(
            choiceContext,
            flipCard,
            sourcePlay);

        return new FlipResult(
            flipped,
            consumed.Count,
            level);
    }

    private static async Task<FlipResult> ResolveSpectrum(
        PlayerChoiceContext choiceContext,
        CardModel sourceCard,
        CardPlay sourcePlay,
        IReadOnlyList<AceColor> consumed,
        int levelBonus)
    {
        if (consumed.Count == 0)
            return default;

        var level = Math.Clamp(
            1 + levelBonus,
            1,
            MaxSpectrumLevel);

        var flipModel = GetSpectrumFlipCard(level);

        if (flipModel is null)
            return new FlipResult(
                null,
                consumed.Count,
                level,
                IsSpectrum: true);

        var flipCard =
            sourceCard.CombatState.CreateCard(
                flipModel,
                sourceCard.Owner);

        await AutoPlayFlipCard(
            choiceContext,
            flipCard,
            sourcePlay);

        return new FlipResult(
            null,
            consumed.Count,
            level,
            IsSpectrum: true);
    }

    private static CardModel? GetColorFlipCard(
        AceColor color,
        int level)
    {
        return (color, level) switch
        {
            // Red: Burst line
            (AceColor.Red, 1) => ModelDb.Card<Burst>(),
            (AceColor.Red, 2) => ModelDb.Card<SuperBurst>(),
            (AceColor.Red, 3) => ModelDb.Card<MegaBurst>(),
            (AceColor.Red, 4) => ModelDb.Card<GigaBurst>(),
            (AceColor.Red, 5) => ModelDb.Card<UltimateBurst>(),

            // White: Stop line
            // Type-0 called this Black Card, but your mod uses White.
            (AceColor.White, 1) => ModelDb.Card<ShortStop>(),
            (AceColor.White, 2) => ModelDb.Card<LongStop>(),
            (AceColor.White, 3) => ModelDb.Card<MegaStop>(),
            (AceColor.White, 4) => ModelDb.Card<GigaStop>(),
            (AceColor.White, 5) => ModelDb.Card<Fullstop>(),

            // Blue: Charge line
            (AceColor.Blue, 1) => ModelDb.Card<Barrier>(),
            (AceColor.Blue, 2) => ModelDb.Card<SuperBarrier>(),
            (AceColor.Blue, 3) => ModelDb.Card<MegaBarrier>(),
            (AceColor.Blue, 4) => ModelDb.Card<GigaBarrier>(),
            (AceColor.Blue, 5) => ModelDb.Card<Aegis>(),

            // Yellow: Force line
            (AceColor.Yellow, 1) => ModelDb.Card<Charge>(),
            (AceColor.Yellow, 2) => ModelDb.Card<SuperCharge>(),
            (AceColor.Yellow, 3) => ModelDb.Card<MegaCharge>(),
            (AceColor.Yellow, 4) => ModelDb.Card<GigaCharge>(),
            (AceColor.Yellow, 5) => ModelDb.Card<Overdrive>(),

            _ => null
        };
    }

    private static CardModel? GetSpectrumFlipCard(int level)
    {
        return level switch
        {
            1 => ModelDb.Card<JackpotShot>(),
            2 => ModelDb.Card<TrueJackpotShot>(),
            _ => null
        };
    }

    private static async Task AutoPlayFlipCard(
        PlayerChoiceContext choiceContext,
        CardModel flipCard,
        CardPlay sourcePlay)
    {
        var target = sourcePlay.Target;

        if (target is null || target.IsDead)
        {
            target = null;
        }

        await CardCmd.AutoPlay(
            choiceContext,
            flipCard,
            target,
            skipCardPileVisuals: false);
    }
    
    public static CardModel? Preview(Player player)
    {
        if (Stock.IsRainbow(player))
            return GetSpectrumFlipCard(1);

        var color = Stock.Majority(player);

        if (color is null)
            return null;

        var level = Math.Min(
            Stock.Count(player, color.Value),
            4);

        return GetColorFlipCard(
            color.Value,
            level);
    }
}