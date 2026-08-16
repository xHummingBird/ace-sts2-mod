using MegaCrit.Sts2.Core.Entities.Cards;

namespace Ace.AceCode.Cards.Ancient;

public class Fullstop() : AceFlipCard(0, CardType.Skill,
    CardRarity.Ancient, TargetType.AnyEnemy)
{
    
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
}