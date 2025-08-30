using UnityEngine;

[DisallowMultipleComponent]
public sealed class BoneID : MonoBehaviour
{
    [SerializeField, Tooltip("Eindeutige ID dieses Knochens (leer = GameObject-Name)")]
    private string id;

    public string Id => id;

    // Wird aufgerufen, wenn das Script neu hinzugef�gt wird oder via Inspector "Reset".
    private void Reset() => SetDefaultIfEmpty();

    // Wird im Editor ausgef�hrt, wenn sich etwas �ndert (Name, Inspector�)
    private void OnValidate() => SetDefaultIfEmpty();

    // Fallback zur Laufzeit
    private void Awake() => SetDefaultIfEmpty();

    private void SetDefaultIfEmpty()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = gameObject.name;   // Standard: Name des GameObjects
    }
}
