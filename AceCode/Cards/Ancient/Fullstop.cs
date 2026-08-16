using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Ace.AceCode.Cards.Ancient;

public class Fullstop() : AceFlipCard(0, CardType.Skill,
    CardRarity.Ancient, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enemies = CombatState.HittableEnemies;
        foreach (var enemy in enemies)
            await CreatureCmd.Stun(enemy);
    }
}