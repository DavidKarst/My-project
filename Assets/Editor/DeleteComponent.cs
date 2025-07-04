using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DeleteComponent : EditorWindow
{
    MonoScript chosenScript;

    [MenuItem("Tools/Delete Component XR Grab Interactable ")]
    static void OpenWindow()
    {
        GetWindow<DeleteComponent>("Delete Component");
    }


}
