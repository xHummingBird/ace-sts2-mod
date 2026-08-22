using Ace.AceCode.Mechanics;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Ace.AceCode.Cards.Uncommon;

// gain 1 strength and 1 dexterity. If all 4 cards in stock are different, gain 2 more strength and dexterity. Retain on upgrade)
public class VermilionVow() : AceCard(
    2,
    CardType.Power,
    CardRarity.Uncommon,
    TargetType.Self)
{
    protected override bool ShouldGlowGoldInternal => (Stock.IsRainbow(Owner));
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(1m),
        new PowerVar<DexterityPower>(1m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var amount = 1m;

        if (Stock.IsRainbow(Owner))
            amount += 2m;

        await PowerCmd.Apply<StrengthPower>(
            choiceContext,
            Owner.Creature,
            amount,
            Owner.Creature,
            this);

        await PowerCmd.Apply<DexterityPower>(
            choiceContext,
            Owner.Creature,
            amount,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}
