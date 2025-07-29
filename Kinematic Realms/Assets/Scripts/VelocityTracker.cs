using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityTracker : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [System.NonSerialized] public Vector3 velocityVector;
    private Vector3 velocity;

    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        if(_rigidbody == null)
        {
            print("Rigid body component missing! Disabling Velocity Script.");
            this.enabled = false;
            return;
        }
        velocity = _rigidbody.linearVelocity;
    }
    private void FixedUpdate()
    {
        velocity = _rigidbody.linearVelocity;
    }
}