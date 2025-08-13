using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Interact : MonoBehaviour
{
    private void Awake()
    {
        var si = GetComponent<XRSimpleInteractable>();
        si.hoverEntered.AddListener(_ => Debug.Log("HOverd"));
        si.activated.AddListener(_ => Debug.Log("Activate " + name));
        si.selectEntered.AddListener(_ => Debug.Log("Select " + name));

    }
}
