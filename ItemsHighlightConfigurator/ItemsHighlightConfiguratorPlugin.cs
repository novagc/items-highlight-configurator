using BepInEx;
using ItemsHighlightConfigurator.Controllers;
using ItemsHighlightConfigurator.Services;
using R2API.Utils;
using RiskOfOptions;

// ReSharper disable MemberCanBePrivate.Global

namespace ItemsHighlightConfigurator;

[BepInDependency("com.rune580.riskofoptions")]
[BepInDependency(R2API.R2API.PluginGUID)]
[BepInPlugin(Guid, Name, Version)]
[NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
public class ItemsHighlightConfiguratorPlugin : BaseUnityPlugin
{
    public const string Guid = Author + "." + Name;
    public const string Author = "NovaGC";
    public const string Name = "ItemsHighlightConfigurator";
    public const string Version = "1.0.0";

    public TierColorConfigsController ConfigsController { get; private set; }
    
    public void Awake()
    {
        Log.Init(Logger);
        ConfigsController = new TierColorConfigsController(Config);
        
        On.RoR2.ItemTierCatalog.Init += OnItemTierCatalogInit;
        
        ModSettingsManager.SetModDescription("Меняет цвет обводки предметов");
    }

    private void OnItemTierCatalogInit(On.RoR2.ItemTierCatalog.orig_Init orig)
    {
        orig();
        ConfigsController.Init();
    }
}

