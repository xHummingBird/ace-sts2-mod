using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

public class Memento() : AceWhiteCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<StrengthPower>(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<StrengthPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<StrengthPower>().UpgradeValueBy(1m);
    }
}
