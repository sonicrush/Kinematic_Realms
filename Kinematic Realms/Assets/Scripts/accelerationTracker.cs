using UnityEngine;
using System.Collections;

public class AccelerationExtrapolation : MonoBehaviour
{
    public GameObject targetObject;
    private Vector3 _previousVelocity;
    private Vector3 _presentVelocity;
    private Rigidbody _rigidbody;
    [System.NonSerialized] public Vector3 AccelerationVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	IEnumerator PerSecond()
	{
		float seconds = 0.1f;
		while(true) 
		{
			_previousVelocity = _presentVelocity;
			//I wonder if there's a property in rigidbody that just holds the changing vector. That could be used instead of this function.
			_presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
			AccelerationVector.x = (_presentVelocity.x - _previousVelocity.x) / seconds;
			AccelerationVector.y = (_presentVelocity.y - _previousVelocity.y) / seconds;
			AccelerationVector.z = (_presentVelocity.z - _previousVelocity.z) / seconds;
			yield return new WaitForSeconds(seconds);
		}
	}
    void Start()
    { 
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
        AccelerationVector = new Vector3(0f, 0f, 0f);
        StartCoroutine(PerSecond());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
