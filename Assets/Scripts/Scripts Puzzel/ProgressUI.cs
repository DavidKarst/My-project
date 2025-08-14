// ProgressUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image bar;                 // Deine Image-Komponente (Filled)

    [Header("Verhalten")]
    [SerializeField] float decayPerSecond = 0.03f; // wie schnell es sinkt
    [SerializeField] float addOnSnap = 0.12f;  // wie viel bei Snap dazu kommt

    [Header("Optionale Events")]
    public System.Action onEmpty;               // z. B. Game Over
    public System.Action onFull;                // z. B. Bonus

    void Reset() => bar = GetComponent<Image>();

    void OnEnable() => BoneMeta.OnAnyBoneSnapped += HandleSnap;
    void OnDisable() => BoneMeta.OnAnyBoneSnapped -= HandleSnap;

    void Update()
    {
        if (!bar) return;

        // kontinuierlich abbauen
        float v = bar.fillAmount - decayPerSecond * Time.deltaTime;
        bar.fillAmount = Mathf.Clamp01(v);

        if (bar.fillAmount <= 0f) onEmpty?.Invoke();
        if (bar.fillAmount >= 1f) onFull?.Invoke();
    }

    void HandleSnap(BoneMeta meta)
    {
        if (!bar) return;

        // bei Snap auffüllen
        float v = bar.fillAmount + addOnSnap;
        bar.fillAmount = Mathf.Clamp01(v);
    }
}
