using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class LastStand() : AceBlueCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BufferPower>(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CommonActions.ApplySelf<BufferPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Power<BufferPower>().UpgradeValueBy(1m);
    }
}
