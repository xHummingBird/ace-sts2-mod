using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

// gain 1 strength and 1 dexterity. If all 4 cards in stock are different, gain 2 more strength and dexterity. Retain on upgrade)
public class VermilionVow() : AceWhiteCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RegenPower>(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<RegenPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<RegenPower>().UpgradeValueBy(1m);
    }
}
