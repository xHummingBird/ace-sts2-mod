using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Ace.AceCode.Relics;

public class MagiciansDeck() : AceRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override decimal ModifyHandDraw(
        Player player,
        decimal count)
    {
        if (base.Owner != player)
            return count;

        return Stock.Majority(player) == AceColor.Yellow ? count + 1m : count;
    }
}
