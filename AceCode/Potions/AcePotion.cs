using BaseLib.Abstracts;
using BaseLib.Utils;
using Ace.AceCode.Character;

namespace Ace.AceCode.Potions;

[Pool(typeof(AcePotionPool))]
public abstract class AcePotion : CustomPotionModel;