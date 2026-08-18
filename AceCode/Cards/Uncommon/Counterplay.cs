using Ace.AceCode.Powers;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Ace.AceCode.Cards.Uncommon;

public class Counterplay() : AceBlueCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<CounterPlayPower>(5m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<CounterPlayPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<CounterPlayPower>().UpgradeValueBy(3m);
    }
}
