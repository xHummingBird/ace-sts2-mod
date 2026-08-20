using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

public class TwinDeal() : AceCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
        [
        ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
        AceStaticHoverTip.Majority
    ];

    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (Stock.IsRainbow(Owner))
        {
            SfxCmd.Play("res://Ace/sounds/open.wav");
            await Ace.AceCode.Mechanics.Flip.Spectrum(
                choiceContext,
                this,
                play);
        }
        else
        {
            SfxCmd.Play("res://Ace/sounds/open.wav");
            await Ace.AceCode.Mechanics.Flip.Majority(choiceContext, this, play, 0, 2);
        }
        
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
