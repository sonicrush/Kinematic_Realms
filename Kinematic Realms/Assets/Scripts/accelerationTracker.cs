using UnityEngine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject targetObject;
    private Vector3 _previousVelocity;
    private Vector3 _presentVelocity;
    private Rigidbody _rigidbody;
    private double[] _accelerationCurrent = new double[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
        ;
    }

    // Update is called once per frame
    void Update()
    {
        _previousVelocity = _presentVelocity;
        _presentVelocity = _rigidbody.GetPointVelocity(transform.TransformPoint(targetObject.transform.position));
        _accelerationCurrent[0] = _presentVelocity[0] - _previousVelocity[0];
        _accelerationCurrent[1] = _presentVelocity[1] - _previousVelocity[1];
        _accelerationCurrent[2] = _presentVelocity[2] - _previousVelocity[2];
        print("X velocity is : " + _accelerationCurrent[0]);
        print("Y velocity is : " + _accelerationCurrent[1]);
        print("Z velocity is : " + _accelerationCurrent[2]);
    }
}
