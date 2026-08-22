using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Flip;

public class Charge() : AceYellowCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self), IFlipCard
{
    public override bool CanBeGeneratedInCombat => false;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        SfxCmd.Play("res://Ace/sounds/draw.wav");
        await CardPileCmd.Draw(choiceContext, 2, base.Owner);
    }
}