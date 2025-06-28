using UnityEngine;
using UnityEditor;
public class accelerationVector : MonoBehaviour

{
    private AccelerationExtrapolation _accelerationExtraporlator;
    private GameObject _vectorArrowAsset;
    private GameObject _vectorArrow;
    private Transform _vectorArrowTransformComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _accelerationExtraporlator = gameObject.GetComponent<AccelerationExtrapolation>();
        _vectorArrowAsset = Resources.Load<GameObject>("Prefabs/vectorArrow");
        _vectorArrow = Instantiate(_vectorArrowAsset);
        _vectorArrowTransformComponent = _vectorArrow.GetComponent<Transform>();
        _vectorArrowTransformComponent.SetParent(gameObject.GetComponent<Transform>(), false);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
