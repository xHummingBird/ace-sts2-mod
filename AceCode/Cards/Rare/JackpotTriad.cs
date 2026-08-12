using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class JackpotTriad() : AceYellowCard(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new RepeatVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.CardAttack(this, play, hitCount: DynamicVars.Repeat.IntValue).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
