using Ace.AceCode.Character;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;

namespace Ace.AceCode.Relics;

public class PlayingCards() : AceRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    public override CardCreationOptions ModifyCardRewardCreationOptions(
        Player player,
        CardCreationOptions options)
    {
        if (base.Owner != player)
            return options;

        if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
            return options;

        if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
            return options;

        return options.WithCardPools(
            options.CardPools
                .Concat(
                [
                    ModelDb.CardPool<AceBluePool>(),
                    ModelDb.CardPool<AceRedPool>(),
                    ModelDb.CardPool<AceYellowPool>(),
                    ModelDb.CardPool<AceWhitePool>()
                ])
                .Distinct()
        );
    }
}