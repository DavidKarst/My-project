using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public enum Language { German, Latin }
public static class BoneMapper
{
    static Dictionary<string, string> de, la;
    static bool loaded;

    public static Language CurrentLanguage { get; private set; } = Language.German;
    public static void ToggleLanguage()
    {
        CurrentLanguage = (CurrentLanguage == Language.German) ? Language.Latin : Language.German;
        Debug.Log("[BoneMapper] Language: " + CurrentLanguage);
    }

    public static string GetName(string objectName)
    {
        EnsureLoaded();
        var map = (CurrentLanguage == Language.German) ? de : la;
        return (map != null && map.TryGetValue(objectName, out var val) && !string.IsNullOrEmpty(val))
            ? val
            : objectName; // Fallback: originaler Objektname
    }
    static void EnsureLoaded()
    {
        if (loaded) return;
        de = LoadCsv(Path.Combine(Application.streamingAssetsPath, "Bone_Names.csv"));
        la = LoadCsv(Path.Combine(Application.streamingAssetsPath, "Bone_Names_Latin.csv"));
        loaded = true;
    }
    static Dictionary<string, string> LoadCsv(string path)
    {
        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (!File.Exists(path))
        {
            Debug.LogWarning("[BoneMapper] File not found: " + path);
            return dict;
        }

        foreach (var raw in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;
            var line = raw.Trim();
            if (line.StartsWith("#")) continue;

            var sep = line.IndexOf(';');
            if (sep < 0) continue;

            var key = line.Substring(0, sep).Trim().Trim('\uFEFF'); // falls BOM
            var val = line.Substring(sep + 1).Trim();

            // optionalen Header "Object;Name" überspringen
            if (key.Equals("Object", StringComparison.OrdinalIgnoreCase)) continue;

            if (!string.IsNullOrEmpty(key))
                dict[key] = val;
        }
        return dict;
    }
}
