using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityTracker : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [System.NonSerialized] public Vector3 velocityVector;
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        if(_rigidbody == null)
        {
            print("Rigid body component missing! Disabling Velocity Script.");
            this.enabled = false;
            return;
        }
        velocityVector = _rigidbody.linearVelocity;
    }
    private void FixedUpdate()
    {
        velocityVector = _rigidbody.linearVelocity;
    }
}