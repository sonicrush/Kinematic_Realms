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
        float xyAngle = Mathf.Atan2(accelerationNormalized.y, accelerationNormalized.x) * Mathf.Rad2Deg;
        float xyMagnitude = accelerationNormalized.y / Mathf.Cos(xyAngle * Mathf.Deg2Rad);

        float finalAngle = Mathf.Atan2(accelerationNormalized.z, xyMagnitude) * Mathf.Rad2Deg;
        // float totalMagnitude = accelerationNormalized.z / Mathf.Cos(zAngle);
        // vectorArrowScriptComponent.stemLength = Mathf.Abs(totalMagnitude);


        Vector3 xyNormalized = new Vector3(accelerationVector.x, accelerationVector.y, 0).normalized;
        Quaternion rotation1 = Quaternion.Euler(0, 0, xyAngle);
        Quaternion rotation2 = new Quaternion(xyNormalized.x * Mathf.Sin(finalAngle * Mathf.Deg2Rad/2), xyNormalized.y * Mathf.Sin(finalAngle* Mathf.Deg2Rad/2), xyNormalized.z * Mathf.Sin(finalAngle* Mathf.Deg2Rad/2), Mathf.Cos(finalAngle* Mathf.Deg2Rad/2));
        Quaternion rotation3 = rotation2 * rotation1;
        _vectorArrowTransformComponent.rotation = rotation3;
    

    }
}
