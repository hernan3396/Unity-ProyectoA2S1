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
    }

    public void DeathRagdoll(bool value)
    {
        SetEnabled(value);
    }
}
