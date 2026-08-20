using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Flip;

public class ShortStop() : AceWhiteCard(0, CardType.Skill,
    CardRarity.Token, TargetType.AnyEnemy), IFlipCard
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2m),
        new PowerVar<VulnerablePower>(2m)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, play.Target, DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, play.Target, DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
    }
}