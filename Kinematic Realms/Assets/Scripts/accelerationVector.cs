using UnityEngine;
using UnityEditor;
public class accelerationVector : MonoBehaviour

{
    private AccelerationExtrapolation _accelerationExtraporlator;
    private GameObject _vectorArrowAsset;
    private GameObject _vectorArrow;
    private Transform _vectorArrowTransformComponent;
    public int xOffset;
    public int yOffset;
    public int quaterionX;
    public int quaterionY;
    public int quaterionZ;
    public int quaterionW;
    Quaternion quaterionThing;
    Vector3 normalizedVector;
    Vector3 vector3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _accelerationExtraporlator = gameObject.GetComponent<AccelerationExtrapolation>();
        _vectorArrowAsset = Resources.Load<GameObject>("Prefabs/vectorArrow");
        Vector3 vector3 = new Vector3(xOffset, yOffset, 0);
        quaterionThing = new Quaternion(quaterionX, quaterionY, quaterionZ, quaterionW);
        _vectorArrow = Instantiate(_vectorArrowAsset, vector3,quaterionThing, gameObject.GetComponent<Transform>());
        _vectorArrowTransformComponent = _vectorArrow.GetComponent<Transform>();
       
    }

    // Update is called once per frame
    void Update()
    {

        quaterionThing.w = quaterionW;
        vector3.x = xOffset;
        vector3.y = yOffset;
        normalizedVector = _accelerationExtraporlator.AccelerationVector.normalized;
        quaterionThing.x = normalizedVector.x;
        quaterionThing.y = normalizedVector.y;
        quaterionThing.z = normalizedVector.z;

        _vectorArrowTransformComponent.Rotate(vector3);



    }
}
