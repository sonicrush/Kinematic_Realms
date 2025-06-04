using UnityEngine;
using System.Collections;

public class AccelerationExtrapolation : MonoBehaviour
{
    public GameObject targetObject;
    private Vector3 _previousVelocity;
    private Vector3 _presentVelocity;
    private Rigidbody _rigidbody;
    [System.NonSerialized]public double accelerationX;
    [System.NonSerialized]public double accelerationY;
    [System.NonSerialized]public double accelerationZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	IEnumerator perSecond() 
	{
		while(true) 
		{
		_previousVelocity = _presentVelocity;
        _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
        accelerationX = (_presentVelocity[0] - _previousVelocity[0]) / 0.1;
        accelerationY = (_presentVelocity[1] - _previousVelocity[1]) / 0.1;
        accelerationZ = (_presentVelocity[2] - _previousVelocity[2]) / 0.1;
		yield return new WaitForSeconds(0.1f);
		}
	}
    void Start()
    { 
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
        StartCoroutine(perSecond());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
