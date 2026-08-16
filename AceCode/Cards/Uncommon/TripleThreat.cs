using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Ace.AceCode.Cards.Uncommon;

public class TripleThreat() : AceCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
        AceStaticHoverTip.Majority
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
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
            await Ace.AceCode.Mechanics.Flip.Majority(choiceContext, this, play, 0, 3);
        }
        
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}