using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class BoneEntry
{
    public string objectName;
    public string germanName;
}

[System.Serializable]
public class BoneEntryList
{
    public BoneEntry[] entries;
}
public class BoneMapper
{
    static Dictionary<string, string> _map;

    // beim ersten Zugriff laden wir das Mapping
    public static void EnsureLoaded()
    {
        if (_map != null) return;

        _map = new Dictionary<string, string>();



        //  CSV aus StreamingAssets
        var path = System.IO.Path.Combine(Application.streamingAssetsPath, "Bone_Names.csv");
        if (System.IO.File.Exists(path))
        {
            foreach (var line in System.IO.File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) || !line.Contains(";")) continue;
                var parts = line.Split(';');
                _map[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }

    public static string GetGermanName(string objectName)
    {
        EnsureLoaded();
        if (_map.TryGetValue(objectName, out var deutsch))
            return deutsch;
        // fallback, falls nichts drinsteht
        return objectName;
    }
}
