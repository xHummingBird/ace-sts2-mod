using Ace.AceCode.Cards.Ancient;
using Ace.AceCode.Character;
using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Cards.Basic;

// Colorless on purpose. A colored card would stock its own color right after
// the flip, because AfterCardPlayed runs after OnPlay.
public class ShowOfHands()
    : AceCard(2, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy), IFlipCard
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
                play);
        }
        else
        {
            await Ace.AceCode.Mechanics.Flip.Majority(
                choiceContext,
                this,
                play);
        }
        Consume.All(base.Owner);
    }
    
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
