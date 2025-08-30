using UnityEngine;

[DisallowMultipleComponent]
public sealed class SpawnerTag : MonoBehaviour
{
    public BoneSpawnPoint owner;   // gesetzt vom SpawnPoint beim Instanziieren
}
