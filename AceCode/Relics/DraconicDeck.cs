using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Relics;

public class DraconicDeck() : AceRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (base.Owner is not { } owner || dealer != owner.Creature)
            return 0m;

        if (!props.IsPoweredAttack())
            return 0m;

        return Stock.Majority(owner) == AceColor.Red ? 1m : 0m;
    }
}
