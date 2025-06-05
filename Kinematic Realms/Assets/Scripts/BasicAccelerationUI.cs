using System;
using TMPro;
using UnityEngine;

public class BasicAccelerationUI : MonoBehaviour
{
    public GameObject referenceObject;
    private TextMeshPro _text;
    private AccelerationExtrapolation _accelerationExtrapolation;
    
    void Start() 
    { 
        _text = gameObject.GetComponent<TextMeshPro>();
        _accelerationExtrapolation = referenceObject.GetComponent<AccelerationExtrapolation>();
    }

    void Update()
    {

        _text.text = String.Format("Acceleration in:\nX: {0:N1} m/s^2\nY: {1:N1} m/s^2\nZ: {2:N1} m/s^2",
            _accelerationExtrapolation.AccelerationVector.x, _accelerationExtrapolation.AccelerationVector.y,
            _accelerationExtrapolation.AccelerationVector.z);
    }
}