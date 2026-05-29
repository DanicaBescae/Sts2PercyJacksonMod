using BaseLib.Abstracts;
using PercyJackson.PercyJacksonCode.Extensions;
using Godot;

namespace PercyJackson.PercyJacksonCode.Character;

public class PercyJacksonRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => PercyJackson.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}