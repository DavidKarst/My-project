// EndGameUI.cs
using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private string winText = "Gewonnen";
    [SerializeField] private string loseText = "Verloren";

    void Awake()
    {
        if (!label) label = GetComponentInChildren<TextMeshProUGUI>(true);
        gameObject.SetActive(false); // erst am Ende zeigen
    }

    public void ShowWin() => Show(winText);
    public void ShowLose() => Show(loseText);
    public void Hide() => gameObject.SetActive(false);

    private void Show(string text)
    {
        if (label) label.text = text;
        gameObject.SetActive(true);
    }
}

