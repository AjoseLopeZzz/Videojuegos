using System.Collections.Generic;
using UnityEngine;

public static class KeysRepository
{
    private static readonly string FileName = "keysData.txt";
    private static KeysData cache;

    private static void EnsureLoaded()
    {
        if (cache != null) return;

        if (DataFiles.TryLoadJson<KeysData>(FileName, out var loaded))
        {
            cache = loaded;
            if (cache.unlocked == null) cache.unlocked = new List<string>();
        }
        else
        {
            cache = new KeysData { unlocked = new List<string>() };
            DataFiles.SaveJson(FileName, cache);
        }
    }

    public static bool IsUnlocked(string id)
    {
        EnsureLoaded();
        return cache.unlocked != null && cache.unlocked.Contains(id);
    }

    public static void SetUnlocked(string id, bool value)
    {
        EnsureLoaded();

        if (value)
        {
            if (!cache.unlocked.Contains(id))
                cache.unlocked.Add(id);
        }
        else
        {
            cache.unlocked.Remove(id);
        }

        DataFiles.SaveJson(FileName, cache);
    }

    public static void ClearAll()
    {
        cache = new KeysData { unlocked = new List<string>() };
        DataFiles.SaveJson(FileName, cache);
    }
}
