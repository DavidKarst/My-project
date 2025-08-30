using Puzzle; // falls du BodyPart in Namespace hast
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BoneMeta : MonoBehaviour
{
    public static event System.Action<BoneMeta> OnAnyBoneSnapped;
    [Header("Zuordnung")]
    public BodyPart part;
    public Transform snapTarget;     // -> Anchor an Torben Ziel

    [Header(" Sicht beim Einrasten ")]
    public GameObject showOnSnap;    // -> Bone bei Torben Ziel (oder dessen Root)
    public bool hideThisOnSnap = true;

    XRGrabInteractable grab;
    Rigidbody rb;
    bool snapped;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();
    }
    public PartKind Kind => part switch
    {
        BodyPart.RightArm or BodyPart.LeftArm => PartKind.Arm,
        BodyPart.RightLeg or BodyPart.LeftLeg => PartKind.Leg,
        BodyPart.Skull => PartKind.Skull,
        _ => PartKind.Body
    };
    public void Snap()
    {
        if (snapped)
        {
            snapped = true;
        }

        if (!snapTarget)
        {
            Debug.LogWarning($"[{name}] SnapTarget fehlt.");
            return;
        }

        // 1) Teil einrasten
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        transform.SetPositionAndRotation(snapTarget.position, snapTarget.rotation);
        transform.SetParent(snapTarget);

        // 2) A-Bone sichtbar schalten
        if (showOnSnap)
        {
            // falls GO deaktiviert war:
            showOnSnap.SetActive(true);

            // falls nur Renderer deaktiviert war:
            foreach (var r in showOnSnap.GetComponentsInChildren<Renderer>(true))
                r.enabled = true;
        }

        // 3) Geworfenes Teil optional verstecken/deaktivieren
        if (hideThisOnSnap)
        {
            if (grab) grab.enabled = false;
            foreach (var c in GetComponentsInChildren<Collider>())
                c.enabled = false;
            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
        }

        OnAnyBoneSnapped?.Invoke(this);
    }
}
