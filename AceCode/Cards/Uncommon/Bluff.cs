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

//Apply 1 vulnerable and for each white card stocked.
public class Bluff() : AceWhiteCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(1m),
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Stock,
        HoverTipFactory.FromPower<VulnerablePower>(),
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        decimal amount =
            Stock.Count(Owner, AceColor.White);

        await PowerCmd.Apply<VulnerablePower>(
            choiceContext,
            play.Target,
            amount,
            Owner.Creature,
            this);
    }


    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
