using Ace.AceCode.Mechanics;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Rare;

public class VermilionBird() : AceRedCard(2, CardType.Power, CardRarity.Rare, TargetType.Self), IConsumeCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<StrengthPower>(2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        decimal strength = DynamicVars.Strength.BaseValue + Stock.Count(base.Owner, AceColor.Red);
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, strength, base.Owner.Creature, this);
        Consume.OfColor(base.Owner, AceColor.Red);
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
