using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Animator _anim;
    private Rigidbody[] _rb;

    private void Start()
    {
        _rb = transform.GetComponentsInChildren<Rigidbody>();
        _anim = GetComponent<Animator>();
        SetEnabled(false);
    }

    private void SetEnabled(bool enabled)
    {
        bool isKinematic = !enabled;
        foreach (Rigidbody rigidbody in _rb)
        {
            rigidbody.isKinematic = isKinematic;
        }

        _anim.enabled = !enabled;
        GetComponentInParent<CapsuleCollider>().enabled = !enabled;
        GetComponentInParent<Entity>().enabled = !enabled;

        if (TryGetComponent(out WaypointsMovement waypointsMovement))
            waypointsMovement.enabled = !enabled;
    }

    public void DeathRagdoll()
    {
        SetEnabled(true);
    }
}
