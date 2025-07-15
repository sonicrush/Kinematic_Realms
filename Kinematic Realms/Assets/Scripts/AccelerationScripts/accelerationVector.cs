using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
public class AccelerationVector : MonoBehaviour

{
    private AccelerationExtrapolation _accelerationExtraporlator;
    public GameObject vectorArrow;
    private Transform _vectorArrowTransformComponent;
    private vectorArrowObject vectorArrowScriptComponent;
    public int xOffset;
    public int yOffset;
    public int zOffset;

    Vector3 accelerationVector;
    void Start()
    {
        _accelerationExtraporlator = gameObject.GetComponent<AccelerationExtrapolation>();
        GameObject _vectorArrowAsset = Resources.Load<GameObject>("Prefabs/vectorArrow");
        vectorArrow = Instantiate(_vectorArrowAsset);
        _vectorArrowTransformComponent = vectorArrow.GetComponent<Transform>();
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = vectorArrow.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(xOffset, yOffset, zOffset));
        vectorArrowScriptComponent = vectorArrow.GetComponent<vectorArrowObject>();

    }

    // Update is called once per frame
    void Update()
    {
        accelerationVector = _accelerationExtraporlator.AccelerationVector;
        Vector3 accelerationNormalized = accelerationVector.normalized;
        // print("Acceleration Vector : " + accelerationVector);
        // print("Acceleration normalized : " + accelerationNormalized);
        // float xyAngle = Mathf.Atan2(accelerationNormalized.y, accelerationNormalized.x) * Mathf.Rad2Deg;
        // float xyMagnitude = accelerationNormalized.x / Mathf.Cos(xyAngle);
        // float zAngle = Mathf.Atan2(xyMagnitude, accelerationNormalized.z) * Mathf.Rad2Deg;
        // float zMagnitude = accelerationNormalized.z / Mathf.Cos(zAngle);
        _vectorArrowTransformComponent.rotation = Quaternion.LookRotation(accelerationNormalized);
    

    }
}
