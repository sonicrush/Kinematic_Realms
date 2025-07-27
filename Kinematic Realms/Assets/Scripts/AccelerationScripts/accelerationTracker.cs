using Unity.VisualScripting;
using UnityEngine;

public class AccelerationTracker : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Vector3 _previousVelocity;
    [System.NonSerialized] public Vector3 AccelerationVector;
    [System.NonSerialized] public int currentUses;


    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        AccelerationVector = new Vector3(0f, 0f, 0f);
        _previousVelocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        _velocity = _rigidbody.linearVelocity;
        AccelerationVector = (_velocity - _previousVelocity) / Time.fixedDeltaTime;
        _previousVelocity = _velocity;
    }
    public static AccelerationTracker TrackAcceleration(GameObject gameObject)
    {
        AccelerationTracker trackerComponent = gameObject.GetOrAddComponent<AccelerationTracker>();
        trackerComponent.currentUses++;
        return trackerComponent;

    }
    public void StopTrackingAcceleration()
    {
        if (currentUses <= 0)
            Destroy(this);
        else
            currentUses--;

    }
    
}
