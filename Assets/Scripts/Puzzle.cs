using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;



[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class Puzzle : MonoBehaviour
{
    [Tooltip("Maximaler Abstand zum Snap‑Punkt, damit es einfädelt.")]
    public float snapDistance = 0.05f;

    XRGrabInteractable grab;
    Rigidbody rb;

    Vector3 originalPosition;
    Quaternion originalRotation;
    Transform originalParent;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        originalParent = transform.parent;
        // in Welt‑Koordinaten (falls Parent bewegt wird, könntest du auch local speichern)
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Listener, der feuert, wenn du das Objekt loslässt
        grab.selectExited.AddListener(OnReleased);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        // Abstand zum Ziel bestimmen
        float dist = Vector3.Distance(transform.position, originalPosition);
        if (dist <= snapDistance)
        {
            // sofort auf die Original‑Pose snappen
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            // wieder als Child anhängen
            transform.SetParent(originalParent, true);

            // physikalisch „festfrieren“
            rb.isKinematic = true;
            // Greifen deaktivieren (optional)
            grab.enabled = false;
        }
    }

    // Falls du das Puzzle nochmal neu starten willst, könntest du eine Reset‑Funktion hinzufügen:
    public void ResetPiece()
    {
        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.isKinematic = false;
        grab.enabled = true;
    }
}
