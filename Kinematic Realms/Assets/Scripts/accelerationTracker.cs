using UnityEngine;
using System.Collections;

public class AccelerationExtrapolation : MonoBehaviour
{
    private Vector3 _previousVelocity;
    private Vector3 _presentVelocity;
    private Rigidbody _rigidbody;
    [System.NonSerialized] public Vector3 AccelerationVector;

    void Start()
    { 
	    _rigidbody = gameObject.GetComponent<Rigidbody>();
	    _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(gameObject.transform.position));
	    AccelerationVector = new Vector3(0f, 0f, 0f);
	    StartCoroutine(PerTime(0.1f));
    }

    IEnumerator PerTime(float seconds)
	{
		while(true) 
		{
			_previousVelocity = _presentVelocity;
			//If there's a property in rigidbody that holds the vector, do refactor the script to avoid calling the function below.
			_presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(gameObject.transform.position));
			AccelerationVector.x = (_presentVelocity.x - _previousVelocity.x) / seconds;
			AccelerationVector.y = (_presentVelocity.y - _previousVelocity.y) / seconds;
			AccelerationVector.z = (_presentVelocity.z - _previousVelocity.z) / seconds;
			yield return new WaitForSeconds(seconds);
		}
	}
}
