using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneStartPositionSet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var startSet = other.GetComponentInParent<BoneStartPosition>();
        if (startSet != null)
        {
            startSet.ResetToStart();
        }
    }
}
