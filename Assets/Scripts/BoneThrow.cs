using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]
public class BoneThrow : MonoBehaviour
{
    Rigidbody rb;
    XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Event‑Handler registrieren
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        // sauber wieder abmelden
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    /// <summary>
    /// Wird aufgerufen, sobald der Knochen gegriffen wird.
    /// Macht den Rigidbody kinematisch und deaktiviert Gravity.
    /// </summary>
    void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    /// <summary>
    /// Wird aufgerufen, sobald der Knochen losgelassen wird.
    /// Reaktiviert Physics und setzt die Controller‑Velocity auf den Rigidbody.
    /// </summary>
    void OnRelease(SelectExitEventArgs args)
    {
        // Physics wieder aktivieren
        rb.isKinematic = false;
        rb.useGravity = true;

        //// Interactor in XRBaseControllerInteractor casten
        //if (args.interactorObject is XRBaseControllerInteractor controllerInteractor
        // && controllerInteractor.xrController != null)
        //{

        //    InputDevice device = controllerInteractor.xrController.inputDevice;

        //    if (device.isValid)
        //    {
        //        // Velocity abfragen
        //        Vector3 velocity, angularVelocity;
        //        device.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
        //        device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out angularVelocity);

        //        // Auf den Rigidbody anwenden
        //        rb.velocity = velocity;
        //        rb.angularVelocity = angularVelocity;
        //    }
        //}
    }
}
