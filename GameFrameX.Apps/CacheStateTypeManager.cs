// The copyrights, trademarks, patents, and other related rights of the GameFrameX organization
// and its derivative projects are protected by applicable laws and regulations.
// Usage of this project must comply with relevant laws, regulations, and license requirements.
//
// This project is primarily distributed under the MIT License and Apache License (Version 2.0).
// The license file is located in the LICENSE file in the root directory of the source code tree.
//
// It is prohibited to use this project for activities that endanger national security, disrupt
// social order, or infringe upon the legitimate rights of others. We assume no responsibility
// for any legal disputes or liabilities arising from secondary development based on this project.

using GameFrameX.Foundation.Extensions;
using GameFrameX.Foundation.Hash;

namespace GameFrameX.Apps;

public static class CacheStateTypeManager
{
    private static readonly ConcurrentDictionary<long, Type> HashMap = new ConcurrentDictionary<long, Type>();

    /// <summary>
    /// Initialize scanning of entity state types
    /// </summary>
    public static void Init()
    {
        var assembly = typeof(AppsHandler).Assembly;
        BsonClassMapHelper.SetConvention();

        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            var isImplWithInterface = type.IsImplWithInterface(typeof(ICacheState));
            if (isImplWithInterface)
            {
                HashMap.TryAdd(XxHashHelper.Hash32(type.ToString()), type);
                BsonClassMapHelper.RegisterClass(type);
            }
        }
    }

    /// <summary>
    /// Get type by type ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Type GetType(long id)
    {
        HashMap.TryGetValue(id, out var value);
        return value;
    }
}