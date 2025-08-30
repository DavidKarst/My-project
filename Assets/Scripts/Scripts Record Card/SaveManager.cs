using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("Referenzen")]
    [Tooltip("XR Origin / Player-Rig, das beim Laden versetzt wird.")]
    public Transform playerRig;

    [Tooltip("Liste aller Knochen in der Szene (kann automatisch gefüllt werden).")]
    public List<BoneID> allBones = new List<BoneID>();

    [Header("Datei")]
    public string fileName = "torben_record_save.json";

    [Serializable]
    public class SaveData
    {
        public List<string> collected = new List<string>();
        public Vector3 rigPos;
        public Quaternion rigRot;
        public bool hasRig; // ob pos/rot gesetzt wurden
    }

    SaveData data;

    string SavePath => Path.Combine(Application.persistentDataPath, fileName);

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Falls die Liste leer ist, suche automatisch alle Knochen
        if (allBones == null || allBones.Count == 0)
            allBones = new List<BoneID>(FindObjectsOfType<BoneID>(true));

        data = LoadFromDisk();
        ApplySaveToScene(); // deaktiviert bereits gesammelte Knochen & setzt ggf. Player-Rig
    }

    // --- API für Buttons ---

    public void SaveHere()
    {
        if (data == null) data = new SaveData();
        if (playerRig)
        {
            data.rigPos = playerRig.position;
            data.rigRot = playerRig.rotation;
            data.hasRig = true;
        }
        WriteToDisk(data);
    }

    public void LoadSave()
    {
        data = LoadFromDisk();
        ApplySaveToScene();
    }

    public void ResetAll()
    {
        data = new SaveData();
        WriteToDisk(data);

        // Alle Knochen aktivieren & auf Start zurücksetzen
        foreach (var b in allBones)
        {
            if (!b) continue;
            b.gameObject.SetActive(true);

            var start = b.GetComponent<BoneStartPosition>();
            if (start) start.ResetToStart();
        }
    }

    // Wird von der Ablage aufgerufen
    public void Collect(string boneId)
    {
        if (string.IsNullOrEmpty(boneId)) return;
        if (!data.collected.Contains(boneId))
            data.collected.Add(boneId);
        // optional: autosave
        // WriteToDisk(data);
    }

    // --- intern ---

    void ApplySaveToScene()
    {
        // Player teleportieren
        if (playerRig && data.hasRig)
            playerRig.SetPositionAndRotation(data.rigPos, data.rigRot);

        // Knochen-Visibilität gemäß Save
        var collectedSet = new HashSet<string>(data.collected);
        foreach (var b in allBones)
        {
            if (!b) continue;
            bool collected = collectedSet.Contains(b.Id);
            b.gameObject.SetActive(!collected);
        }
    }

    SaveData LoadFromDisk()
    {
        try
        {
            if (File.Exists(SavePath))
                return JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
        }
        catch (Exception e) { Debug.LogWarning($"Load failed: {e.Message}"); }
        return new SaveData();
    }

    void WriteToDisk(SaveData d)
    {
        try
        {
            var json = JsonUtility.ToJson(d, true);
            File.WriteAllText(SavePath, json);
            // Debug.Log($"Saved -> {SavePath}\n{json}");
        }
        catch (Exception e) { Debug.LogWarning($"Save failed: {e.Message}"); }
    }

#if UNITY_EDITOR
    [ContextMenu("Auto collect all bones in scene")]
    void AutoCollectBones()
    {
        allBones = new List<BoneID>(FindObjectsOfType<BoneID>(true));
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
