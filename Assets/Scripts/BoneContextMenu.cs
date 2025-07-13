using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoneContextMenu : MonoBehaviour
{
    [Tooltip("Prefab für das World‑Space Kontext‑Menü (World‑Canvas mit Text)")]
    public GameObject contextMenu;

    [Tooltip("Offset relativ zum Bone‑Pivot, wo das Menü erscheint")]
    public Vector3 menuOffset = new Vector3(0f, 0.1f, 0f);

    GameObject menuInstance;
    XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    void OnDestroy()
    {
        // Clean‑up
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (contextMenu == null || menuInstance != null)
            return;

        // Menü instanziieren
        menuInstance = Instantiate(contextMenu);
        // Position & Rotation setzen
        menuInstance.transform.position = transform.TransformPoint(menuOffset);
        // Menu soll immer zur Kamera schauen
        Camera cam = Camera.main;
        if (cam != null)
            menuInstance.transform.rotation = Quaternion.LookRotation(menuInstance.transform.position - cam.transform.position);

        // Text befüllen
        var tmp = menuInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = "?";

        // gameObject.name;
    }

    void OnHoverExited(HoverExitEventArgs args)
    {
        if (menuInstance != null)
            Destroy(menuInstance);
    }
}
