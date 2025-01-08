using System;
using System.Linq;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;

namespace ItemsHighlightConfigurator.Controllers;

public class TierColorSyncController: IDisposable
{
    private readonly ConfigEntry<Color> _config;
    private readonly ColorCatalog.ColorIndex _index;
    private readonly ItemTier _tier;

    private int ColorIndex => (int)_index;
    
    public TierColorSyncController(ConfigEntry<Color> config, ColorCatalog.ColorIndex index, ItemTier tier)
    {
        _config = config;
        _index = index;
        _tier = tier;

        if (ColorIndex < 0 || ColorIndex >= ColorCatalog.indexToColor32.Length)
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"{nameof(index)} must be in range [0,{ColorCatalog.indexToColor32.Length}]"
            );
        
        InitSynchronization();
    }

    private void InitSynchronization()
    {
        ColorCatalog.indexToColor32[ColorIndex] = _config.Value;
        _config.SettingChanged += OnColorChanged;
    }
    
    private void UpdatePickupsColor()
    {
        var newColor = _config.Value;
        ColorCatalog.indexToColor32[ColorIndex] = newColor;
        
        foreach (var def in PickupCatalog.allPickups.Where(x => x.itemTier == _tier))
        {
            def.baseColor = newColor;
        }
    }

    private void OnColorChanged(object sender, EventArgs e) => UpdatePickupsColor();
    
    public void Dispose()
    {
        _config.SettingChanged -= OnColorChanged;
    }
}