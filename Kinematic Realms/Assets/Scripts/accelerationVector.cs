using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
public class AccelerationVector : MonoBehaviour

{
    private AccelerationExtrapolation _accelerationExtraporlator;
    private GameObject _vectorArrowAsset;
    public GameObject vectorArrow;
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
        vectorArrow = Instantiate(_vectorArrowAsset);
        _vectorArrowTransformComponent = vectorArrow.GetComponent<Transform>();
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = vectorArrow.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(0, 0, 0));

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
