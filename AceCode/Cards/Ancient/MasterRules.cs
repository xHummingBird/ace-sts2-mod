using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Ace.AceCode.Cards.Ancient;

public class MasterRules() : AceCard(2, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy), IFlipCard
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
        AceStaticHoverTip.Majority,
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play) 
    {
        if (Stock.IsRainbow(Owner))
        {
            await Ace.AceCode.Mechanics.Flip.Spectrum(
                choiceContext,
                this,
                play,
                1);
        }
        else
        {
            await Ace.AceCode.Mechanics.Flip.Majority(
                choiceContext,
                this,
                play,
                1);
        }
        Consume.All(base.Owner);
    }
    
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}