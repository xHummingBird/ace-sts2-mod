using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

//Gain 10 block. If current majority is blue, gain the same amount next turn. Use BlockNextTurn power. Stock check is only during activation 
public class EvasiveManeuver() : AceBlueCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IStockingCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(10, ValueProp.Move)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Majority
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardBlock(this, play);

        if (Stock.Majority(Owner) == AceColor.Blue)
        {
            await PowerCmd.Apply<BlockNextTurnPower>(
                choiceContext,
                Owner.Creature,
                DynamicVars.Block.BaseValue,
                Owner.Creature,
                this);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}
