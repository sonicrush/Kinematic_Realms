using UnityEngine;

public class velocityVector : MonoBehaviour
{
    private GameObject _vectorArrowOffset;
    private GameObject _vectorArrow;
    private Transform _vectorArrowTransformComponent;
    private Rigidbody rb;
    public int xOffset, yOffset;
    public int quaternionX, quaternionY, quaternionZ, quaternionW;
    Quaternion quaternionThing;
    Vector3 normalizedVector;
    Vector3 vector3;
    Vector3 velocityExtrapolator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _vectorArrowOffset = Resources.Load<GameObject>("Prefabs/vectorArrow");
        Vector3 vector3 = new Vector3(xOffset, yOffset, 0);
        quaternionThing = new Quaternion(quaternionX, quaternionY, quaternionZ, quaternionW);
        _vectorArrow = Instantiate(_vectorArrowOffset, vector3, quaternionThing, gameObject.GetComponent<Transform>());
        _vectorArrowTransformComponent = _vectorArrow.GetComponent<Transform>();
        velocityExtrapolator = rb.linearVelocity;

}

// Update is called once per frame
void Update()
    {
        quaternionThing.w = quaternionW;
        vector3.x = xOffset;
        vector3.y = yOffset;
        normalizedVector = velocityExtrapolator.normalized;
        quaternionThing.x = normalizedVector.x;
        quaternionThing.y = normalizedVector.y;
        quaternionThing.z = normalizedVector.z;

        _vectorArrowTransformComponent.Rotate(vector3);

    }
}
