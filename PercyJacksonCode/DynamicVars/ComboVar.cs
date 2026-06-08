using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PercyJackson.PercyJacksonCode.DynamicVars;

public class ComboVar : DynamicVar
{
    public const string Key = "ComboX";

    public ComboVar(decimal comboCount) : base(Key, comboCount)
    {
        this.WithTooltip();
    }
}