using UnityEngine;

public sealed class BoneSpawnPoint : MonoBehaviour
{
    [SerializeField] Transform drop;     // optional; sonst this.transform
    [SerializeField] Vector3 localOffset; // feines Auswerfen/Abstand
    GameObject current;

    public bool IsFree => current == null;

    public GameObject Spawn(GameObject prefab)
    {
        // Wenn schon belegt, gib das bestehende Objekt zurück
        if (current != null) return current;

        var parent = drop ? drop : transform;

        // Als Kind instanzieren, damit localPosition/localRotation sinnvoll sind
        current = Instantiate(prefab, parent);
        current.name = prefab.name;                 // entfernt automatisch „(Clone)“

        // Lokaler Offset + gewünschte Ausrichtung relativ zum Parent
        current.transform.localPosition = localOffset;
        current.transform.localRotation = Quaternion.identity; // oder parent.rotation, wie gewünscht

        // Spawner-Referenz an das Teil hängen
        var tag = current.GetComponent<SpawnerTag>() ?? current.gameObject.AddComponent<SpawnerTag>();
        tag.owner = this;

        return current;
    }

    public void FreeFrom(GameObject spawned)
    {
        if (current == spawned) current = null;
    }

    public void ForceClear()
    {
        if (current) Object.Destroy(current);
        current = null;
    }
    public void ReturnToSpawn(GameObject go, bool resetVelocity = true)
    {
        if (current == null || go != current) return;

        var rb = go.GetComponent<Rigidbody>();
        if (rb)
        {
            // kurz kinematisch, damit das Umsetzen keine Physik-Artefakte erzeugt
            if (resetVelocity)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            rb.useGravity = false;   // nicht fallen
            rb.isKinematic = true;   // im Spawn „schweben“
            rb.Sleep();
        }

        var parent = drop ? drop : transform;
        go.transform.SetParent(parent);
        go.transform.localPosition = localOffset;
        go.transform.localRotation = Quaternion.identity;

        if (rb) rb.isKinematic = false;
    }



}
