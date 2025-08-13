using UnityEngine;
using Puzzle;

[RequireComponent(typeof(Collider))]
public class PartSnapZone : MonoBehaviour
{
    public BodyPart accepts;          // welcher Part darf hier?
    public bool snapOnEnter = true;   // sofort snappen, wenn korrekt

    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void OnTriggerEnter(Collider other)
    {
        var meta = other.GetComponentInParent<BoneMeta>();
        if (meta == null) return;

        // nur korrekter Part darf snappen
        if (meta.part != accepts) return;

        if (snapOnEnter) meta.Snap();
    }
}
