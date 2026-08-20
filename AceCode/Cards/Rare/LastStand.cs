using Ace.AceCode.Extensions;
using Ace.AceCode.Mechanics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class LastStand() : AceBlueCard(1, CardType.Power, CardRarity.Rare, TargetType.Self), IConsumeCard
{
    protected override bool IsPlayable => Stock.Count(Owner) == Stock.MaxSlots;
    protected override bool ShouldGlowGoldInternal => IsPlayable;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(1m)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        AceStaticHoverTip.Consume,
        AceStaticHoverTip.Unstockable
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        Consume.All(Owner);
        await CommonActions.ApplySelf<BufferPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<BufferPower>().UpgradeValueBy(1m);
    }
}
