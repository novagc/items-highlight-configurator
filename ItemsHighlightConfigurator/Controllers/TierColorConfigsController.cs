using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BepInEx.Configuration;
using ItemsHighlightConfigurator.Models.Exceptions;
using ItemsHighlightConfigurator.Services;
using ItemsHighlightConfigurator.Utils;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace ItemsHighlightConfigurator.Controllers;

public class TierColorConfigsController: IDisposable
{
    public ConfigFile PluginConfigFile { get; set; }
    
    private readonly Dictionary<ItemTier, TierColorSyncController> _syncControllers = [];
    private readonly Dictionary<ItemTier, Color> _originalTierColors = [];

    public ReadOnlyDictionary<ItemTier, Color> OriginalTierColors => new(_originalTierColors);
    
    public TierColorConfigsController(ConfigFile pluginConfigFile)
    {
        PluginConfigFile = pluginConfigFile;
    }
    
    public void Init()
    {
        foreach (var tierDef in ContentManager.itemTierDefs)
        {
            try
            {
                CreateTierColorConfigs(tierDef);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    private void CreateTierColorConfigs(ItemTierDef tierDef)
    {
        var tier = tierDef.tier;

        if (_syncControllers.ContainsKey(tier))
            throw new DuplicateTierException(tier);
        
        var originalColor = ColorCatalog.GetColor(tierDef.colorIndex);
        
        var currentColorConfig = PluginConfigFile.Bind<Color>(
            "Items",
            $"{tier} color",
            originalColor,
            $"Цвет обводки для предметов класса {tier}"
        );
        
        var originalColorConfig = PluginConfigFile.Bind<Color>(
            "Items",
            $"{tier} original color",
            originalColor,
            $"[НЕИЗМЕНЯЕМОЕ] Оригинальный цвет обводки для предметов класса {tier}"
        );

        originalColorConfig.Value = originalColor;
        originalColorConfig.SettingChanged += (_, _) => originalColorConfig.Value = originalColor;

        currentColorConfig.RegisterConfigOption();
        originalColorConfig.RegisterConfigOption();

        var syncController = currentColorConfig.SyncColorConfig(tier);
        
        _syncControllers.Add(tier, syncController);
        _originalTierColors.Add(tier, originalColor);
    }

    public void Dispose()
    {
        foreach (var controller in _syncControllers.Values)
        {
            controller.Dispose();
        }
    }
}