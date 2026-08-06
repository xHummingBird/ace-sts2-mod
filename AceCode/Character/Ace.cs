using Ace.AceCode.Cards.Basic;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Ace.AceCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Ace.AceCode.Character;


public class Ace : PlaceholderCharacterModel
{
    public const string CharacterId = "Ace";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 65;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeRed>(),
        ModelDb.Card<StrikeRed>(),
        ModelDb.Card<StrikeBlue>(),
        ModelDb.Card<StrikeWhite>(),
        ModelDb.Card<StrikeYellow>(),
        ModelDb.Card<DefendRed>(),
        ModelDb.Card<DefendBlue>(),
        ModelDb.Card<DefendBlue>(),
        ModelDb.Card<DefendYellow>(),
        ModelDb.Card<DefendWhite>(),
        //ModelDb.Card<CutCards>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
    ];
    
    public static bool IsRed(CardModel card)
        => card.Pool is AceRedPool;

    public static bool IsBlue(CardModel card)
        => card.Pool is AceBluePool;

    public static bool IsYellow(CardModel card)
        => card.Pool is AceYellowPool;

    public static bool IsWhite(CardModel card)
        => card.Pool is AceWhitePool;

    public override CardPoolModel CardPool => ModelDb.CardPool<AceCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<AceRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<AcePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override CustomEnergyCounter? CustomEnergyCounter =>
        new CustomEnergyCounter(EnergyCounterPaths, new Color(0.2f, 0.2f, 0.2f), new Color(1f, 1f, 1f));
    
    private string EnergyCounterPaths(int i)
    {
        return i switch
        {
            1 => "charui/big_energy.png".ImagePath(),
            _ => "charui/blank.png".ImagePath()
        };
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}