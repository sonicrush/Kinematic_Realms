using UnityEngine;

public class AccelerationExtrapolation : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _force;
    [System.NonSerialized] public Vector3 AccelerationVector;

    void Start()
    { 
	    _rigidbody = gameObject.GetComponent<Rigidbody>();
	    AccelerationVector = new Vector3(0f, 0f, 0f);
    }
    private void FixedUpdate()
    {
	    _force = _rigidbody.GetAccumulatedForce();
	    AccelerationVector.x = _force.x / _rigidbody.mass;
	    if (_rigidbody.useGravity)
	    {
		    AccelerationVector.y = (_force.y / _rigidbody.mass) - 9.8f;
	    }
	    else
			AccelerationVector.y = _force.y / _rigidbody.mass;
	    AccelerationVector.z = _force.z / _rigidbody.mass;
    }
}
