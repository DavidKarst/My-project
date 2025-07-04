using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AddComponent : EditorWindow
{
    MonoScript chosenScript;

    [MenuItem("Tools/Add Component To Selected Hierarchy")]
    static void OpenWindow()
    {
        GetWindow<AddComponent>("Add Component");
    }

    void OnGUI()
    {
        GUILayout.Label("Komponente auswählen, dann auf ‚Add to Children‘ klicken", EditorStyles.boldLabel);

        chosenScript = (MonoScript)EditorGUILayout.ObjectField(
            "Component Script",
            chosenScript,
            typeof(MonoScript),
            false
        );

        if (GUILayout.Button("Add XRGrabInteractable"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                {
                    var child = t.gameObject;
                    XRGrabInteractable existingGrab = child.GetComponent<XRGrabInteractable>();
                    if (existingGrab != null)
                    {
                        Undo.DestroyObjectImmediate(existingGrab);
                    }
                    XRGrabInteractable grab = Undo.AddComponent<XRGrabInteractable>(child);
                    Undo.RecordObject(grab, "Enable Dynamic Attach");
                    grab.useDynamicAttach = true;
                    grab.attachEaseInTime = 0f;
                    grab.movementType = XRBaseInteractable.MovementType.Instantaneous;
                    EditorUtility.SetDirty(grab);
                }



            }
        }
        Debug.Log("XRGrabInteractable wurde allen selektierten Objekten und ihren Kindern hinzugefügt.");
    }
}


