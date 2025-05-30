using UnityEngine;


public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject targetObject;
    private double _previousVelocity;
    public 
    private double _presentVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _previousVelocity = _presentVelocity;
        _presentVelocity = thisRigidBody.GetPointVelocity(Transform.TransformPoint.(targetObject.transform.position));
    }
}
