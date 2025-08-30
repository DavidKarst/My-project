using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class BoneThrow : MonoBehaviour
{
    [Header("Shoot Settings")]
    [Tooltip("Geschwindigkeit, mit der der Knochen abgeschossen wird")]
    public float shootForce = 15f;

    Rigidbody rb;
    XRGrabInteractable grabInteractable;
    BoneStartPosition startPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPos = GetComponent<BoneStartPosition>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.throwOnDetach = false;
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }



    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    void OnRelease(SelectExitEventArgs args)
    {
        if (startPos != null)
        {
            startPos.EnablePhysics();
        }

        // 2) Abschussrichtung = Controller-Forward
        Vector3 shootDir;
        if (args.interactorObject is XRBaseControllerInteractor ci)
        {
            // Wenn ein AttachTransform gesetzt ist, nutze dessen Forward,
            // sonst die Controller-Transform-Forward
            shootDir = (ci.attachTransform != null)
                ? ci.attachTransform.forward
                : ci.transform.forward;
        }
        else if (Camera.main != null)
        {
            // Fallback: Kamerablickrichtung
            shootDir = Camera.main.transform.forward;
        }
        else
        {
            // Letzter Rückgriff: Objekt-Forward
            shootDir = transform.forward;
        }

        // 3) Velocity vergeben
        Vector3 finalVel = shootDir.normalized * shootForce;
        rb.velocity = finalVel;

        // 4) Log für Debug
        Debug.Log($"[BoneThrow] ShootDir: {shootDir:F2}, Vel: {finalVel:F2}");
    }
}
