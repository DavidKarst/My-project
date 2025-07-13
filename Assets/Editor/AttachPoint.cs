using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachPoint : EditorWindow
{
    [MenuItem("Tools/Generate AttachPoints for Bones")]
    static void OpenWindow() => GetWindow<AttachPoint>();

    void OnGUI()
    {
        if (GUILayout.Button("AttachPoints für selektierte Bones erzeugen"))
        {
            foreach (var go in Selection.gameObjects)
                CreateAttachPoint(go);
        }
    }

    static void CreateAttachPoint(GameObject go)
    {
        // nur wenn XRGrabInteractable drauf sitzt
        var grab = go.GetComponent<XRGrabInteractable>();
        if (grab == null) return;

        // 1) ermittel Lokal‑Center über Mesh oder Collider
        Vector3 localCenter = Vector3.zero;
        var mf = go.GetComponent<MeshFilter>();
        if (mf != null)
        {
            localCenter = mf.sharedMesh.bounds.center;
        }
        else
        {
            var col = go.GetComponent<Collider>();
            if (col != null)
            {
                Vector3 worldCenter = col.bounds.center;
                localCenter = go.transform.InverseTransformPoint(worldCenter);
            }
        }

        // 2) new GameObject anlegen
        var ap = new GameObject("AttachPoint");
        Undo.RegisterCreatedObjectUndo(ap, "Create AttachPoint");
        ap.transform.SetParent(go.transform, false);
        ap.transform.localPosition = localCenter;
        ap.transform.localRotation = Quaternion.identity;

        // 3) im Interactable zuweisen
        Undo.RecordObject(grab, "Assign AttachPoint");
        grab.attachTransform = ap.transform;
        EditorUtility.SetDirty(grab);
    }
}
