using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteObjekt : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image progress;
    private int maxPoints = 254;
    private int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            Destroy(other.gameObject);
            counter++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (text != null)
        {
            text.text = $"{counter} / {maxPoints}";
        }
        if (progress != null)
        {
            progress.fillAmount = (float)counter / maxPoints;

        }
    }
}