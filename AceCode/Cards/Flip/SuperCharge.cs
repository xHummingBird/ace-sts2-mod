using Ace.AceCode.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Ace.AceCode.Cards.Flip;

public class SuperCharge() : AceYellowCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self), IFlipCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PlayerCmd.GainEnergy(1, base.Owner);
        await CardPileCmd.Draw(choiceContext, 2, base.Owner);
    }
}