using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BoneStartPosition : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    private Transform startParent;
    private Rigidbody rb;
    Collider col;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        startParent = transform.parent;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        SetToStart(true);
    }


    public void EnablePhysics()
    {
        SetToStart(false);
    }

    void SetToStart(bool b)
    {
        rb.isKinematic = b;
        rb.useGravity = !b;
        rb.detectCollisions = true;

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
        SetToStart(true);
    }
}
