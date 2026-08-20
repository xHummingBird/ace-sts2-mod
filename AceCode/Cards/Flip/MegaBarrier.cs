using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Cards.Flip;

public class MegaBarrier() : AceBlueCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self), IFlipCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<PlatingPower>(8m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal plating = DynamicVars["PlatingPower"].BaseValue + Stock.Count(base.Owner, AceColor.Blue);
        
        await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner.Creature, plating, base.Owner.Creature, this);
    }
}