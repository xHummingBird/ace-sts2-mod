using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

public class MarkTheDeck() : AceWhiteCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<WeakPower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WeakPower>().UpgradeValueBy(1m);
    }
}
