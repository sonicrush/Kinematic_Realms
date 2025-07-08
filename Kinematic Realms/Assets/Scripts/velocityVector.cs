using UnityEngine;

public class velocityVector : MonoBehaviour
{
    private GameObject _vectorArrow;
    private Transform _vectorArrowTransformComponent;
    private Rigidbody rb;

    public string arrowPrefabPath = "Prefabs/vectorArrow";

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Load the prefab and instantiate as child of this GameObject (the Ball)
        GameObject arrowPrefab = Resources.Load<GameObject>(arrowPrefabPath);
        _vectorArrow = Instantiate(arrowPrefab, transform);
        _vectorArrowTransformComponent = _vectorArrow.transform;
    }

    void Update()
    {
        if (rb == null || _vectorArrowTransformComponent == null)
            return;

        Vector3 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            Vector3 velocityDir = velocity.normalized;

            // Calculate scale based on speed (tune these values)
            float speed = velocity.magnitude;
            float maxSpeed = 10f;
            float minScale = 0.1f;
            float maxScale = 3f;
            float scaleX = Mathf.Clamp(speed / maxSpeed, minScale, maxScale);

            // Apply rotation in XY plane
            float angle = Mathf.Atan2(velocityDir.y, velocityDir.x) * Mathf.Rad2Deg;
            _vectorArrowTransformComponent.rotation = Quaternion.Euler(0, 0, angle);

            // Apply scaling only on X axis (arrow length)
            _vectorArrowTransformComponent.localScale = new Vector3(scaleX, 1f, 1f);

            // Adjust position so arrow base stays attached to ball
            float arrowBaseLength = 1f; // adjust if your arrow prefab is longer/shorter
            float arrowLength = arrowBaseLength * scaleX;
            _vectorArrowTransformComponent.position = transform.position + velocityDir * (arrowLength / 2f);
        }
        else
        {
            // Shrink arrow when velocity is near zero
            _vectorArrowTransformComponent.localScale = new Vector3(0f, 1f, 1f);
        }
    }


}
