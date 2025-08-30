using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ReturnToSpawnZone : MonoBehaviour
{
    [SerializeField] bool resetVelocity = true;

    void OnTriggerEnter(Collider other)
    {
        // falls der Collider ein Child ist: in Parent hochgehen
        var tag = other.GetComponentInParent<SpawnerTag>();
        if (tag == null || tag.owner == null) return;

        // Knochen zum Spawn zurücksetzen
        tag.owner.ReturnToSpawn(tag.gameObject, resetVelocity);
    }


}

