using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AccelerationUIDisplay : MonoBehaviour
{
    public GameObject referenceObject;
    public TextMeshProUGUI textObject;
    private AccelerationTracker _accelerationExtrapolation;
    
    void Start() 
    {
        if (textObject == null)
        {
            textObject = gameObject.GetComponent<TextMeshProUGUI>();
            if (textObject == null)
                textObject = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        _accelerationExtrapolation = referenceObject.GetOrAddComponent<AccelerationTracker>();
    }

    void Update()
    {
        textObject.text = String.Format("Acceleration in:\nX: {0:N1} m/s^2\nY: {1:N1} m/s^2\nZ: {2:N1} m/s^2",
            _accelerationExtrapolation.AccelerationVector.x, _accelerationExtrapolation.AccelerationVector.y,
            _accelerationExtrapolation.AccelerationVector.z);
    }
}