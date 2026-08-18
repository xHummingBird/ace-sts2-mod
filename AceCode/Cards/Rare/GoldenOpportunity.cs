using Ace.AceCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Ace.AceCode.Cards.Rare;

public class GoldenOpportunity() : AceYellowCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GoldenOpportunityPower>(1m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<GoldenOpportunityPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<GoldenOpportunityPower>().UpgradeValueBy(1m);
    }
}
