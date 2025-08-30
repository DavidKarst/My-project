using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public sealed class BoneRoundManager : MonoBehaviour
{
    [Header("Quellen & Ziele")]
    [Tooltip("Wurzel von 'Torben B' – hier werden die Vorlagenknochen gesucht.")]
    public Transform sourceRoot;

    [Tooltip("Vier Spawnpunkte unter deinem 'Spawner' GameObject.")]
    public List<BoneSpawnPoint> spawnPoints = new List<BoneSpawnPoint>();

    [Header("Runde")]
    [Min(1)] public int bonesPerRound = 50;
    [SerializeField, Min(0f)] float spawnInterval = 0.15f;

    [Header("Events")]
    public UnityEvent onRoundWon;

    Queue<GameObject> queue;    // prefabs, die noch kommen
    int snappedCount;
    void Start() => StartRound();
    void OnEnable() => BoneMeta.OnAnyBoneSnapped += OnBoneSnapped;
    void OnDisable() => BoneMeta.OnAnyBoneSnapped -= OnBoneSnapped;

    // ---------- Bedienung ----------

    [ContextMenu("Collect SpawnPoints (children)")]
    public void CollectSpawnPoints()
    {
        spawnPoints = GetComponentsInChildren<BoneSpawnPoint>(true).ToList();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [ContextMenu("Start Round")]
    public void StartRound()
    {
        StopAllCoroutines();
        ClearAllSpawnPoints();
        snappedCount = 0;

        var pool = FindAllBonePrefabs();
        if (pool.Count == 0)
        {
            Debug.LogWarning("BoneRoundManager: Keine BoneMeta unter 'sourceRoot' gefunden.");
            return;
        }

        // 50 einzigartige zufällig ziehen
        var n = Mathf.Min(bonesPerRound, pool.Count);
        var picked = pool.OrderBy(_ => Random.value).Take(n);
        queue = new Queue<GameObject>(picked);

        StartCoroutine(Feeder());
    }

    [ContextMenu("Reset Round")]
    public void ResetRound()
    {
        StopAllCoroutines();
        ClearAllSpawnPoints();
        snappedCount = 0;
    }

    void ClearAllSpawnPoints()
    {
        foreach (var p in spawnPoints) if (p) p.ForceClear();
    }

    // ---------- Spawnen ----------

    IEnumerator Feeder()
    {
        var freeBuffer = new List<BoneSpawnPoint>(spawnPoints.Count);

        while (queue != null && queue.Count > 0)
        {
            // freie Punkte sammeln
            freeBuffer.Clear();
            foreach (var p in spawnPoints)
                if (p && p.IsFree) freeBuffer.Add(p);

            // wenn frei -> befüllen
            foreach (var p in freeBuffer)
            {
                if (queue.Count == 0) break;
                var prefab = queue.Dequeue();
                p.Spawn(prefab);
                if (spawnInterval > 0f)
                    yield return new WaitForSeconds(spawnInterval);
            }

            // keine Slots frei? kurz warten
            if (freeBuffer.Count == 0)
                yield return null;
        }
    }

    // ---------- Zählen / Gewinnen ----------

    void OnBoneSnapped(BoneMeta meta)
    {
        if (!meta) return;

        // Slot freigeben, in dem der Knochen lag
        var tag = meta.GetComponent<SpawnerTag>();
        if (tag && tag.owner)
            tag.owner.FreeFrom(meta.gameObject);

        snappedCount++;
        if (snappedCount >= bonesPerRound)
        {
            onRoundWon?.Invoke();
            StopAllCoroutines();
        }
    }

    // ---------- Helpers ----------

    /// Sucht alle Vorlagenknochen unter Torben B:
    /// Kriterium: hat BoneMeta (deine Puzzle-Komponente).
    List<GameObject> FindAllBonePrefabs()
    {
        if (!sourceRoot) return new List<GameObject>();
        return sourceRoot.GetComponentsInChildren<BoneMeta>(true)
                         .Select(m => m.gameObject)
                         .Distinct()
                         .ToList();
    }
}
