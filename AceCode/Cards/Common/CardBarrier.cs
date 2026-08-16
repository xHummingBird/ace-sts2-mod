using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

public class CardBarrier() : AceBlueCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self), IStockingCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(5, ValueProp.Move),
        new DynamicVar("BlockMultiplier", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardBlock(this, play);
        decimal blockAmount = DynamicVars["BlockMultiplier"].BaseValue * Stock.Count(base.Owner, AceColor.Blue);
        await CreatureCmd.GainBlock(base.Owner.Creature, blockAmount, ValueProp.Unpowered, play, false);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlockMultiplier"].UpgradeValueBy(1m);
    }
}