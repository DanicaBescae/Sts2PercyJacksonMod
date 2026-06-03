using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using PercyJackson.PercyJacksonCode.Utils;

namespace PercyJackson.PercyJacksonCode.Patches;

[HarmonyPatch(typeof(AssetSets), nameof(AssetSets.CommonAssets), MethodType.Getter)]
internal static class AssetSetsCommonAssetPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref IReadOnlySet<string> __result)
    {
        __result = __result.Concat(PercyJacksonResource.AssetPaths).ToHashSet();
    }
}