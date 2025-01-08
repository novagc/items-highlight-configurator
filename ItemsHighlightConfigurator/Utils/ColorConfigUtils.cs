using System;
using BepInEx.Configuration;
using ItemsHighlightConfigurator.Controllers;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using UnityEngine;

namespace ItemsHighlightConfigurator.Utils;

public static class ColorConfigUtils
{
    public static TierColorSyncController SyncColorConfig(this ConfigEntry<Color> config, ItemTier tier)
    {
        var tierDef = ItemTierCatalog.GetItemTierDef(tier);

        if (tierDef is null) throw new ArgumentException("Unsupported tier", nameof(tier));
        
        return new TierColorSyncController(config, tierDef.colorIndex, tier);
    }
    
    public static void RegisterConfigOption(this ConfigEntry<Color> config) => ModSettingsManager.AddOption(new ColorOption(config));
}