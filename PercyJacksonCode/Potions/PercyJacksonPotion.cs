using BaseLib.Abstracts;
using BaseLib.Utils;
using PercyJackson.PercyJacksonCode.Character;

namespace PercyJackson.PercyJacksonCode.Potions;

[Pool(typeof(PercyJacksonPotionPool))]
public abstract class PercyJacksonPotion : CustomPotionModel;