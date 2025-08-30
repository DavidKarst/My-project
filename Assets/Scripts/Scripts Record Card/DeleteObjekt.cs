using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class DeleteObjekt : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image progress;

    [SerializeField] int maxPoints = 254;
    int counter;

    void Reset()
    {
        // Sicherheit: sorgt dafür, dass der Collider ein Trigger ist
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // Robuste Erkennung: suche den Bone-Root über ein Kennscript
        // z.B. jeder Knochen hat BoneStartPosition 
        var boneRoot = other.GetComponentInParent<BoneStartPosition>(); //
        if (boneRoot != null)
        {
            Destroy(boneRoot.gameObject);
            counter++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (text) text.text = $"{counter} / {maxPoints}";

        if (progress)
        {
            // Falls im Inspector noch nicht gesetzt: auf Filled umstellen
            if (progress.type != Image.Type.Filled)
                progress.type = Image.Type.Filled;

            progress.fillMethod = Image.FillMethod.Horizontal;
            progress.fillOrigin = 0; // von links
            progress.fillAmount = (float)counter / maxPoints;
        }
    }
}
