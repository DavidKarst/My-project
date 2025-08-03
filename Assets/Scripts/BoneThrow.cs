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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

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
        // Physics wieder aktivieren
        rb.isKinematic = false;
        rb.useGravity = true;

        // Richtung bestimmen: 
        // Entweder Objekt-Forward oder Controller-Forward, falls du das möchtest
        Vector3 shootDir = transform.forward;

        // Optional: Wenn du wirklich den Controller-Forward nutzen willst:
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
            shootDir = controllerInteractor.attachTransform.forward;

        // Geschwindigkeit setzen
        rb.velocity = shootDir.normalized * shootForce;
        // Optional: noch Drehimpuls
        // rb.angularVelocity = Random.insideUnitSphere * someSpinAmount;
    }
}
