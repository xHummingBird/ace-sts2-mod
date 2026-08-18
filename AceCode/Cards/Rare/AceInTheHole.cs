using Ace.AceCode.Cards.Ancient;
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

public class AceInTheHole() : AceCard(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy), IFlipCard, IConsumeCard
{
    
    protected override bool IsPlayable => Stock.Count(Owner) == Stock.MaxSlots;
    protected override bool ShouldGlowGoldInternal => IsPlayable;

    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<JackpotShot>(),
        AceStaticHoverTip.Flip,
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Consume.All(base.Owner);
        var js = CombatState.CreateCard<JackpotShot>(base.Owner);
        await CardCmd.AutoPlay(choiceContext, js, play.Target);
        await AceRelicTriggers.OnFlipPayoff(base.Owner, Stock.MaxSlots, isSpectrum: true);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
