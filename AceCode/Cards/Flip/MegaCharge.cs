using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Cards.Flip;

public class MegaCharge() : AceYellowCard(0, CardType.Skill,
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
        await PowerCmd.Apply<FreeSkillPower>(choiceContext, base.Owner.Creature, 2m, base.Owner.Creature, this);
    }
}