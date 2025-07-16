using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoneWindow : MonoBehaviour
{
    [Header("Referenzen")]
    public GameObject infoPanel;    // dein Canvas/Panel
    public TMP_Text infoText;     // TextMeshPro Text, oder benutze UnityEngine.UI.Text

    XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // Events hookup
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
        grab.activated.AddListener(OnActivate);
    }

    void OnDestroy()
    {
        // sauber wieder abmelden
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
        grab.activated.RemoveListener(OnActivate);
    }

    // 1) beim Greifen
    void OnGrab(SelectEnterEventArgs args)
    {
        infoPanel.SetActive(true);
        infoText.text = "?";

        // Optional: Panel an der Controller‑Position ausrichten
        var interactorTransform = args.interactorObject.transform;
        infoPanel.transform.position = interactorTransform.position + interactorTransform.forward * 0.1f;
        infoPanel.transform.rotation = interactorTransform.rotation;
    }

    // 2) beim Trigger‑Drücken
    void OnActivate(ActivateEventArgs args)
    {
        // Args.InteractableObject ist dein Bone; hier genügt aber "this"
        //   infoText.text = gameObject.name;
        infoText.text = BoneMapper.GetGermanName(gameObject.name);
    }

    // 3) beim Loslassen
    void OnRelease(SelectExitEventArgs args)
    {
        infoPanel.SetActive(false);
    }
}
