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

namespace Ace.AceCode.Cards.Rare;

public class LastCard() : AceYellowCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self), IFlipCard
{
    protected override bool ShouldGlowGoldInternal => PileType.Draw.GetPile(base.Owner).Cards.Count == 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Level", 1m)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Flip,
        AceStaticHoverTip.Unstockable
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SfxCmd.Play("res://Ace/sounds/open.wav");
        if (PileType.Draw.GetPile(base.Owner).Cards.Count == 0) 
            await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Yellow, DynamicVars["Level"].IntValue);
        else await Ace.AceCode.Mechanics.Flip.Color(choiceContext, this, play, AceColor.Yellow);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Level"].UpgradeValueBy(1);
    }
}
