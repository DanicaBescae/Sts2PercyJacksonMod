using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using PercyJackson.PercyJacksonCode.Cards.Basic;
using PercyJackson.PercyJacksonCode.Extensions;

namespace PercyJackson.PercyJacksonCode.Powers;

public class DisarmPower: TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Disarm>();
    
    public string CustomPackedIconPath => "shackle.png".PowerImagePath();
    public string CustomBigIconPath => "shackle.png".BigPowerImagePath();

    protected override bool IsPositive => false;
}