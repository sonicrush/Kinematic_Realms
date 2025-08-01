using UnityEngine;
using UnityEngine.Animations;

public class velocityVector : MonoBehaviour
{
    private VelocityTracker _velocityTracker;
    public GameObject vectorArrow;
    private Transform _vectorArrowTransformComponent;
    private vectorArrowObject vectorArrowScriptComponent;
    public int xOffset;
    public int yOffset;
    public int zOffset;
    public bool dontAttachToObject;
    public bool dontChangeByMagnitude;
    public float vectorInitialLength; 
    public float vectorMaxMagnitude;
    public float unitScalar; // TODO: Implement dynamic scaling
    public string vectorArrowPrefabPath = "Prefabs/vectorArrow";
    public Material vectorArrowMaterial;

    public bool disableVectorOnSpawn;


    private Vector3 velocityVector3;
    void Start()
    {
        _velocityTracker = gameObject.GetComponent<VelocityTracker>();

        if (vectorArrow == null)
        {
            GameObject _vectorArrowAsset = Resources.Load<GameObject>(vectorArrowPrefabPath);
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
        if (dontChangeByMagnitude)
        {
            vectorMaxMagnitude = 0;
        }
        else if (vectorMaxMagnitude == 0)
        {
            vectorMaxMagnitude = 1;
        }

        if (unitScalar == 0)
        {
            unitScalar = 1;
        }

        vectorMaxMagnitude = 3f;
        if (vectorArrowMaterial != null)
        {
            vectorArrow.GetComponent<MeshRenderer>().material = vectorArrowMaterial;
        }
        if (disableVectorOnSpawn)
        {
            vectorArrow.SetActive(false);
        }
    }


    void Update()
    {
        velocityVector3 = _velocityTracker.velocityVector;
        Vector3 velocityNormalized = velocityVector3.normalized;
        if (velocityNormalized.z <= 0.000001f) // For when only 2D motion occurs
        {
            float angleXtoYRadians = Mathf.Atan2(velocityNormalized.y, velocityNormalized.x);
            _vectorArrowTransformComponent.rotation = new Quaternion(0, 0, Mathf.Sin(angleXtoYRadians / 2), Mathf.Cos(angleXtoYRadians / 2));
            ApplyMagnitude(velocityVector3.magnitude);
            return;
        }


        Vector3 xzNormalized = new Vector3(velocityVector3.x, 0, velocityVector3.z).normalized;


        float angleXtoZRadians = Mathf.Atan2(velocityNormalized.z, velocityNormalized.x);

        float angleXZtoFinalVectorRadians;
        Vector3 crossXZtoY;
        if (xzNormalized.x == 0 && xzNormalized.z == 0) //To prevent cross product from losing information when no lateral movement occurs 
        {
            crossXZtoY = Vector3.forward;
            angleXZtoFinalVectorRadians = Mathf.Atan2(velocityNormalized.y, velocityNormalized.x);
        }
        else
        {
            crossXZtoY = Vector3.Cross(xzNormalized, velocityNormalized).normalized;
            angleXZtoFinalVectorRadians = Vector3.Angle(xzNormalized, velocityNormalized) * Mathf.Deg2Rad;
        }

        Quaternion rotation1 = new Quaternion(0, -1 * Mathf.Sin(angleXtoZRadians / 2), 0, Mathf.Cos(angleXtoZRadians / 2));
        Quaternion rotation2 = new Quaternion(crossXZtoY.x * Mathf.Sin(angleXZtoFinalVectorRadians / 2), crossXZtoY.y * Mathf.Sin(angleXZtoFinalVectorRadians / 2), crossXZtoY.z * Mathf.Sin(angleXZtoFinalVectorRadians / 2), Mathf.Cos(angleXZtoFinalVectorRadians / 2));
        Quaternion rotation3 = rotation2 * rotation1;
        _vectorArrowTransformComponent.rotation = rotation3;
        ApplyMagnitude(velocityVector3.magnitude);


    }

    void ApplyMagnitude(float magnitude)
    {
        //if(dynamicScaling)
        //{
        //    unitScalar = Some math to make it dynamic with magnitude;
        //}

        vectorArrowScriptComponent.stemLengthBonus = Mathf.Clamp(velocityVector3.magnitude / unitScalar, 0f, vectorMaxMagnitude);
        //vectorArrowScriptComponent.stemLength = vectorInitialLength;
        //Uncomment when trying to find the perfect initial length for an object.
    }
}
