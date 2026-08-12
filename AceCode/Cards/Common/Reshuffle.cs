using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Common;

// Draw 3 cards. Put 2 cards on your Hand on top of your draw pile. Draw 4 for upgraded
public class Reshuffle() : AceYellowCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.Apply<WeakPower>(choiceContext, this, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<WeakPower>().UpgradeValueBy(1m);
    }
}
