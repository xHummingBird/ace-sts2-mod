using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class DeckOfFate() : AceWhiteCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ArtifactPower>(3m), new PowerVar<RegenPower>(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<ArtifactPower>(choiceContext, this);
        await CommonActions.ApplySelf<RegenPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<ArtifactPower>().UpgradeValueBy(1m);
        DynamicVars.Power<RegenPower>().UpgradeValueBy(1m);
    }
}
