using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Ace.AceCode.Extensions;

public static class AceStaticHoverTip
{
    public static readonly IHoverTip Stock = new HoverTip(
        new LocString("static_hover_tips", "ACE_STOCK.title"),
        new LocString("static_hover_tips", "ACE_STOCK.description")
    );

    public static readonly IHoverTip Flip = new HoverTip(
        new LocString("static_hover_tips", "ACE_FLIP.title"),
        new LocString("static_hover_tips", "ACE_FLIP.description")
    );

    public static readonly IHoverTip Unstockable = new HoverTip(
        new LocString("static_hover_tips", "ACE_UNSTOCKABLE.title"),
        new LocString("static_hover_tips", "ACE_UNSTOCKABLE.description")
    );

    public static readonly IHoverTip Majority = new HoverTip(
        new LocString("static_hover_tips", "ACE_MAJORITY.title"),
        new LocString("static_hover_tips", "ACE_MAJORITY.description")
    );

    public static readonly IHoverTip Consume = new HoverTip(
        new LocString("static_hover_tips", "ACE_CONSUME.title"),
        new LocString("static_hover_tips", "ACE_CONSUME.description")
    );

    public static readonly IHoverTip Red = new HoverTip(
        new LocString("static_hover_tips", "ACE_RED.title"),
        new LocString("static_hover_tips", "ACE_RED.description")
    );

    public static readonly IHoverTip Blue = new HoverTip(
        new LocString("static_hover_tips", "ACE_BLUE.title"),
        new LocString("static_hover_tips", "ACE_BLUE.description")
    );

    public static readonly IHoverTip Yellow = new HoverTip(
        new LocString("static_hover_tips", "ACE_YELLOW.title"),
        new LocString("static_hover_tips", "ACE_YELLOW.description")
    );

    public static readonly IHoverTip White = new HoverTip(
        new LocString("static_hover_tips", "ACE_WHITE.title"),
        new LocString("static_hover_tips", "ACE_WHITE.description")
    );
}