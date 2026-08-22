using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Ace.AceCode.Cards.Flip;

public class GigaBarrier() : AceBlueCard(0, CardType.Skill,
    CardRarity.Token, TargetType.Self), IFlipCard
{
    public override bool CanBeGeneratedInCombat => false;
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<BufferPower>(choiceContext, this);
    }
}