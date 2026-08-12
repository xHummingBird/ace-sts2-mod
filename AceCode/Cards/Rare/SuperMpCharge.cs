using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class SuperMpCharge() : AceYellowCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(4m), new PowerVar<DexterityPower>(4m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
        await CommonActions.ApplySelf<DexterityPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<StrengthPower>().UpgradeValueBy(1m);
        DynamicVars.Power<DexterityPower>().UpgradeValueBy(1m);
    }
}
