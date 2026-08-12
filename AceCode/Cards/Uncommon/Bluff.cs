using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

public class Bluff() : AceCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VulnerablePower>(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<VulnerablePower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<VulnerablePower>().UpgradeValueBy(1m);
    }
}
