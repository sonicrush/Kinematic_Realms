using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;
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
    public bool dontAttachToObject;
    public bool dontChangeByMagnitude;
    //public bool dynamicScaling;
    public float vectorInitialLength; // Should be set for when it's no longer visible at magnitude 0
    public float vectorMaxMagnitude;
    public float unitScalar; // TODO: Implement dynamic scaling


    Vector3 accelerationVector;
    void Start()
    {
        _accelerationExtraporlator = gameObject.GetComponent<AccelerationExtrapolation>();

        if (vectorArrow == null)
        {
            GameObject _vectorArrowAsset = Resources.Load<GameObject>("Prefabs/vectorArrow");
            vectorArrow = Instantiate(_vectorArrowAsset);
            if (!dontAttachToObject)
            {
                vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
                vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = vectorArrow.GetComponent<Transform>(), weight = 1 });
                vectorArrow.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(xOffset, yOffset, zOffset));
            }
            else
            {
                vectorArrow.GetComponent<Transform>().Translate(xOffset, yOffset, zOffset);
            }
        }
        _vectorArrowTransformComponent = vectorArrow.GetComponent<Transform>();
        vectorArrowScriptComponent = vectorArrow.GetComponent<vectorArrowObject>();
        vectorArrowScriptComponent.stemLength = vectorInitialLength;
        if(dontChangeByMagnitude)
        {
            vectorMaxMagnitude = 0;
        }
        else if(vectorMaxMagnitude == 0)
        {
            vectorMaxMagnitude = 1;
        }

        if(unitScalar == 0)
        {
            unitScalar = 1;
        }

        

    }

    
    void Update()
    {
        accelerationVector = _accelerationExtraporlator.AccelerationVector;
        Vector3 accelerationNormalized = accelerationVector.normalized;
        if(accelerationNormalized.z == 0) // For when only 2D motion occurs
        {
            float angleXtoYRadians = Mathf.Atan2(accelerationNormalized.y, accelerationNormalized.x);
            _vectorArrowTransformComponent.rotation = new Quaternion(0, 0, Mathf.Sin(angleXtoYRadians / 2), Mathf.Cos(angleXtoYRadians / 2));
            ApplyMagnitude(accelerationVector.magnitude);
            return;
        }


        Vector3 xzNormalized = new Vector3(accelerationVector.x, 0, accelerationVector.z).normalized;


        float angleXtoZRadians = Mathf.Atan2(accelerationNormalized.z,accelerationNormalized.x);

        float angleXZtoFinalVectorRadians;
        Vector3 crossXZtoY;
        if (xzNormalized.x == 0 && xzNormalized.z == 0) //To prevent cross product from losing information when no lateral movement occurs 
        {
            crossXZtoY = Vector3.forward;
            angleXZtoFinalVectorRadians = Mathf.Atan2(accelerationNormalized.y, accelerationNormalized.x);
        }
        else
        {
            crossXZtoY = Vector3.Cross(xzNormalized, accelerationNormalized).normalized;
            angleXZtoFinalVectorRadians = Vector3.Angle(xzNormalized, accelerationNormalized) * Mathf.Deg2Rad;
        }

        Quaternion rotation1 = new Quaternion(0, -1 * Mathf.Sin(angleXtoZRadians / 2), 0, Mathf.Cos(angleXtoZRadians / 2));
        Quaternion rotation2 = new Quaternion(crossXZtoY.x * Mathf.Sin(angleXZtoFinalVectorRadians / 2), crossXZtoY.y * Mathf.Sin(angleXZtoFinalVectorRadians / 2), crossXZtoY.z * Mathf.Sin(angleXZtoFinalVectorRadians / 2), Mathf.Cos(angleXZtoFinalVectorRadians / 2));
        Quaternion rotation3 = rotation2 * rotation1;
        _vectorArrowTransformComponent.rotation = rotation3;
        ApplyMagnitude(accelerationVector.magnitude);


    }

    void ApplyMagnitude(float magnitude)
    {
        //if(dynamicScaling)
        //{
        //    unitScalar = Some math to make it dynamic with magnitude;
        //}

        vectorArrowScriptComponent.stemLengthBonus = Mathf.Clamp(accelerationVector.magnitude / unitScalar, 0f, vectorMaxMagnitude);
        //vectorArrowScriptComponent.stemLength = vectorInitialLength;
        //Uncomment when trying to find the perfect initial length for an object.
    }
}
