using System.Text.Json;
using GameFrameX.Core.Config;
using GameFrameX.Foundation.Logger;

namespace GameFrameX.Config;

public class ConfigComponent
{
    private readonly ConfigManager _configManager;

    private ConfigComponent()
    {
        _configManager = new ConfigManager();
        Tables = new TablesComponent();
    }

    public static ConfigComponent Instance { get; } = new();

    private TablesComponent Tables { get; }

    public async Task LoadConfig()
    {
        Tables.Init(Instance);
        LogHelper.Info("Load Config Start...");
        Instance.RemoveAllConfigs();
        await Tables.LoadAsync(Loader);
        LogHelper.Info("Load Config End...");
        LogHelper.Info("== load success ==");
    }

    private static async Task<ByteBuf> Loader(string file, bool tag)
    {
        var configBytes = await File.ReadAllBytesAsync($"json/{file}.bytes");
        return ByteBuf.Wrap(configBytes);
    }

    private static async Task<JsonElement> Loader(string file)
    {
        var configJson = await File.ReadAllTextAsync($"json/{file}.json");
        var jsonElement = JsonDocument.Parse(configJson).RootElement;
        return jsonElement;
    }

    /// <summary>
    /// Gets the specified global configuration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetConfig<T>() where T : IDataTable
    {
        if (HasConfig<T>())
        {
            var configName = typeof(T).Name;
            var config = _configManager.GetConfig(configName);
            if (config != null)
            {
                return (T)config;
            }
        }

        return default;
    }

    /// <summary>
    /// Checks whether the specified global configuration exists.
    /// </summary>
    /// <returns>Whether the specified global configuration exists.</returns>
    public bool HasConfig<T>() where T : IDataTable
    {
        var configName = typeof(T).Name;
        return _configManager.HasConfig(configName);
    }

    /// <summary>
    /// Removes the specified global configuration.
    /// </summary>
    /// <returns>Whether the removal was successful.</returns>
    public bool RemoveConfig<T>() where T : IDataTable<T>
    {
        var configName = typeof(T).Name;
        return _configManager.RemoveConfig(configName);
    }

    /// <summary>
    /// Removes all global configurations.
    /// </summary>
    public void RemoveAllConfigs()
    {
        _configManager.RemoveAllConfigs();
    }

    /// <summary>
    /// Add a configuration entry.
    /// </summary>
    /// <param name="configName"></param>
    /// <param name="dataTable"></param>
    public void Add(string configName, IDataTable dataTable)
    {
        _configManager.AddConfig(configName, dataTable);
    }
}