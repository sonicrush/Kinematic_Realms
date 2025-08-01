using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementTest : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private InputAction _up;
    private InputAction _down;
    private InputAction _left;
    private InputAction _right;
    private InputAction _jump;
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _up = InputSystem.actions.FindAction("Player/Forward");
        _down = InputSystem.actions.FindAction("Player/Backward");
        _left = InputSystem.actions.FindAction("Player/Left");
        _right = InputSystem.actions.FindAction("Player/Right");
        _jump = InputSystem.actions.FindAction("Player/Up");
    }
    void FixedUpdate()
    {
        if(_up.IsPressed())
            _rigidbody.AddForce(Vector3.forward * 2f, ForceMode.Force);
        if(_left.IsPressed())
            _rigidbody.AddForce(Vector3.left * 2f, ForceMode.Force);
        if(_down.IsPressed())
            _rigidbody.AddForce(Vector3.back * 2f, ForceMode.Force);
        if(_right.IsPressed())
            _rigidbody.AddForce(Vector3.right * 2f, ForceMode.Force);
        if(_jump.IsPressed())
            _rigidbody.AddForce(Vector3.up * 10.8f, ForceMode.Force);
    }
}
