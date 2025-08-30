// ProgressEndBinder.cs
using UnityEngine;

public class ProgressEndBinder : MonoBehaviour
{
    public ProgressUI progress;
    public EndGameUI endUI;

    void Awake()
    {
        if (progress)
        {
            progress.onEmpty += () => endUI?.ShowLose();
            progress.onFull += () => {/* optional */};
        }
    }
}
