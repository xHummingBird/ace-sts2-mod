using Ace.AceCode.Mechanics;
using Ace.AceCode.Powers;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

public class Breaksight() : AceWhiteCard(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy), IStockingCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<BreaksightPower>(1m),
        new EnergyVar(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<BreaksightPower>(),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<BreaksightPower>(choiceContext, play.Target, DynamicVars["BreaksightPower"].BaseValue, base.Owner.Creature, this);
        await PowerCmd.Apply<FreeAttackPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}