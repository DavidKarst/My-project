using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoneStartPosition : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    private Transform startParent;
    private Rigidbody rb;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startParent = transform.parent;
        rb = GetComponent<Rigidbody>();
    }


    public void ResetToStart()
    {
        if (rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

        }

        transform.SetParent(startParent);
        transform.SetPositionAndRotation(startPos, startRot);
    }
}
