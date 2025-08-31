using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoneWindow : MonoBehaviour
{
    [Header("Referenzen")]
    public GameObject infoPanel;
    public TMP_Text infoText;


    [Header("Follow")]
    public float speed = 20f;
    public float distFromHead = 0.22f;
    public float upOffsest = 0.03f;
    //public Vector3 offSet = new Vector3(0, 0, 0.12f);

    XRGrabInteractable grab;
    Transform followTarget;
    Transform camHead;
    //Quaternion rotOffset = Quaternion.identity;
    //Vector3 posOffset;
    bool revealed;
    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // Events Listener anmelden
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
        grab.activated.AddListener(OnActivate);

        camHead = Camera.main ? Camera.main.transform : null;


        infoText.enableAutoSizing = true;       // passt Schriftgröße an


    }

    void OnDestroy()
    {
        // Listener abmelden
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
        grab.activated.RemoveListener(OnActivate);
    }

    // 1) beim Greifen
    void OnGrab(SelectEnterEventArgs args)
    {
        followTarget = args.interactorObject.transform;
        infoPanel.SetActive(true);
        infoText.text = "?";

    }

    // 2) beim Trigger‑Drücken
    void OnActivate(ActivateEventArgs args)
    {

        infoText.text = BoneMapper.GetName(gameObject.name);
        revealed = true;
    }

    // 3) beim Loslassen
    void OnRelease(SelectExitEventArgs args)
    {
        followTarget = null;
        infoPanel.SetActive(false);
    }
    public void RefreshIfVisible()
    {
        if (infoPanel != null && infoPanel.activeSelf && revealed)
            infoText.text = BoneMapper.GetName(gameObject.name);
    }

    private void LateUpdate()
    {
        if (followTarget == null || camHead == null)
        {
            return;
        }
        // Zielposition berechnen
        Vector3 targetPos = followTarget.position + camHead.forward * distFromHead + camHead.up * upOffsest;
        Quaternion targetRot = Quaternion.LookRotation(targetPos - camHead.position, camHead.up);

        // interpolation
        infoPanel.transform.position = Vector3.Lerp(infoPanel.transform.position, targetPos, Time.deltaTime * speed);
        infoPanel.transform.rotation = Quaternion.Slerp(infoPanel.transform.rotation, targetRot, Time.deltaTime * speed);
    }
}