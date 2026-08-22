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

public class StaticDeal() : AceYellowCard(0, CardType.Skill, CardRarity.Common, TargetType.Self), IFlipCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
        AceStaticHoverTip.Unstockable
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SfxCmd.Play("res://Ace/sounds/open.wav");
        await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Yellow, 0, 1);
    }

    protected override void OnUpgrade()
    {
       RemoveKeyword(CardKeyword.Exhaust);
    }
}
