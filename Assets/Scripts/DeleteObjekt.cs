using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteObjekt : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            Destroy(other.gameObject);
            counter++;
            UpdateTextCounter();
        }
    }

    private void UpdateTextCounter()
    {
        text.text = counter + " / 254";
    }
}
