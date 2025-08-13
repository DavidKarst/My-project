using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageSwitcher : MonoBehaviour
{
    public TMP_Text buttonLabel; // optional

    public void ToggleLanguage()
    {
        BoneMapper.ToggleLanguage();
        if (buttonLabel)
            buttonLabel.text = (BoneMapper.CurrentLanguage == Language.German)
                ? "Sprache: Deutsch" : "Sprache: Latein";

        // OPTIONAL: offene Infofenster manuell auffrischen (ohne Event-System)
        foreach (var w in FindObjectsOfType<BoneWindow>())
            w.RefreshIfVisible();
    }
}
