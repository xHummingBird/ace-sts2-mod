using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Ace.AceCode.Character;
using Ace.AceCode.Extensions;
using Godot;

namespace Ace.AceCode.Relics;

[Pool(typeof(AceRelicPool))]
public abstract class AceRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}