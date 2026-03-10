using System.Collections.Concurrent;
using GameFrameX.Core.Config;

namespace GameFrameX.Config;

/// <summary>
/// Global configuration manager.
/// </summary>
internal sealed partial class ConfigManager : IConfigManager
{
    private readonly ConcurrentDictionary<string, IDataTable> m_ConfigDatas;

    /// <summary>
    /// Initializes a new instance of the global configuration manager.
    /// </summary>
    public ConfigManager()
    {
        m_ConfigDatas = new ConcurrentDictionary<string, IDataTable>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Gets the count of global configuration entries.
    /// </summary>
    public int Count
    {
        get { return m_ConfigDatas.Count; }
    }

    /// <summary>
    /// Checks whether the specified global configuration exists.
    /// </summary>
    /// <param name="configName">The name of the configuration to check.</param>
    /// <returns>Whether the specified global configuration exists.</returns>
    public bool HasConfig(string configName)
    {
        return m_ConfigDatas.TryGetValue(configName, out _);
    }


    /// <summary>
    /// Adds the specified global configuration entry.
    /// </summary>
    /// <param name="configName">The name of the configuration to add.</param>
    /// <param name="configValue">The configuration value.</param>
    /// <returns>Whether the addition was successful.</returns>
    public void AddConfig(string configName, IDataTable configValue)
    {
        var isExist = m_ConfigDatas.TryGetValue(configName, out var value);
        if (isExist)
        {
            return;
        }

        m_ConfigDatas.TryAdd(configName, configValue);
    }

    /// <summary>
    /// Removes the specified global configuration entry.
    /// </summary>
    /// <param name="configName">The name of the configuration to remove.</param>
    public bool RemoveConfig(string configName)
    {
        if (!HasConfig(configName))
        {
            return false;
        }

        return m_ConfigDatas.TryRemove(configName, out _);
    }

    /// <summary>
    /// Gets the specified global configuration entry.
    /// </summary>
    /// <param name="configName">The name of the configuration to get.</param>
    /// <returns>The configuration entry, or null if not found.</returns>
    public IDataTable GetConfig(string configName)
    {
        return m_ConfigDatas.TryGetValue(configName, out var value) ? value : null; //GetConfig()
    }

    /// <summary>
    /// Removes all global configuration entries.
    /// </summary>
    public void RemoveAllConfigs()
    {
        m_ConfigDatas.Clear();
    }
}