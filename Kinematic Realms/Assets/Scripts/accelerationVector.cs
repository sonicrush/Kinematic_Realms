using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
public class AccelerationVector : MonoBehaviour

{
    private AccelerationExtrapolation _accelerationExtraporlator;
    public GameObject vectorArrow;
    private Transform _vectorArrowTransformComponent;
    public int xOffset;
    public int yOffset;
    Vector3 accelerationVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _accelerationExtraporlator = gameObject.GetComponent<AccelerationExtrapolation>();
        GameObject _vectorArrowAsset = Resources.Load<GameObject>("Prefabs/vectorArrow");
        vectorArrow = Instantiate(_vectorArrowAsset);
        _vectorArrowTransformComponent = vectorArrow.GetComponent<Transform>();
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = vectorArrow.GetComponent<Transform>(), weight = 1 });
        vectorArrow.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(0, 0, 0));
        accelerationVector = _accelerationExtraporlator.AccelerationVector;

    }

    // Update is called once per frame
    void Update()
    {
        //Rotation and magnitude
        if (accelerationVector.sqrMagnitude > 0.01f)
        {
            Vector3 accelerationNormalized = accelerationVector.normalized;

            // Calculate scale based on speed
            float speed = accelerationVector.magnitude;
            float maxSpeed = 10f;
            float minScale = 0.1f;
            float maxScale = 3f;
            float scaleX = Mathf.Clamp(speed / maxSpeed, minScale, maxScale);

            // Apply rotation in XY plane
            float angle = Mathf.Atan2(accelerationNormalized.y, accelerationNormalized.x) * Mathf.Rad2Deg;
            _vectorArrowTransformComponent.rotation = Quaternion.Euler(0, 0, angle);

            // Apply scaling only on X axis (arrow length)
            _vectorArrowTransformComponent.localScale = new Vector3(scaleX, 1f, 1f);

            // Adjust position so arrow base stays attached to ball
            float arrowBaseLength = 1f; // adjust if your arrow prefab is longer/shorter
            float arrowLength = arrowBaseLength * scaleX;
            _vectorArrowTransformComponent.position = transform.position + accelerationNormalized * (arrowLength / 2f);
        }
        else
        {
            // Shrink arrow when velocity is near zero
            _vectorArrowTransformComponent.localScale = new Vector3(0f, 1f, 1f);
        }



    }
}
