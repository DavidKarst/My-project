using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[DefaultExecutionOrder(-50)]
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Header("Welche Objekte sollen zurückgesetzt werden?")]
    [Tooltip("Wird die Liste leer gelassen, sucht das Script alle BoneStartPosition in der Szene.")]
    public List<BoneStartPosition> targets = new List<BoneStartPosition>();

    [Header("Optional")]
    [Tooltip("Zusätzliche Objekte, die beim Reset reaktiviert werden sollen (z. B. UI).")]
    public List<GameObject> reactivateAlso = new List<GameObject>();

    void Awake()
    {
        if (targets == null || targets.Count == 0)
            targets = new List<BoneStartPosition>(FindObjectsOfType<BoneStartPosition>(true));
    }


    public void ResetAll()
    {
        // ggf. zusätzliche Objekte wieder aktivieren
        foreach (var go in reactivateAlso)
            if (go) go.SetActive(true);

        // alle Zielobjekte zurücksetzen
        foreach (var start in targets)
        {
            if (!start) continue;

            // Zielobjekt sichtbar machen
            start.gameObject.SetActive(true);

            // auf definierte Start-Transform/Physik zurücksetzen
            start.ResetToStart();

            // Sicherheitsnetz: Physik beruhigen
            var rb = start.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                // Falls dein ResetToStart die Physik wieder aktivieren soll:
                // rb.isKinematic = false; rb.useGravity = true;
            }
        }
    }



#if UNITY_EDITOR
    [ContextMenu("Auto-Fill targets from scene")]
    void AutoFill()
    {
        targets = new List<BoneStartPosition>(FindObjectsOfType<BoneStartPosition>(true));
        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("Reset Now")]
    void ResetNowInEditor() => ResetAll();
#endif
}
