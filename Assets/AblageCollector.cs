using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AblageCollector : MonoBehaviour
{
    [Tooltip("Statt Destroy() den Knochen nur deaktivieren (empfohlen).")]
    public bool disableInsteadOfDestroy = true;

    void OnTriggerEnter(Collider other)
    {
        var bone = other.GetComponentInParent<BoneID>();
        if (!bone) return;

        // im Save vermerken
        SaveManager.Instance.Collect(bone.Id);

        // Knochen „verschwindet“ in der Ablage
        if (disableInsteadOfDestroy) bone.gameObject.SetActive(false);
        else Destroy(bone.gameObject);
    }
}
