using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndScroll : MonoBehaviour
{
    // Reference to the generated InputActions class (replace BallInputActions with your generated class name if different)
    private Ball.Ball_Input_Actions inputActions;

    private Vector3 curScreenPos;
    private Camera camera;
    private bool isDragging;

    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float altScrollSpeed = 3f;

    private Vector3 WorldPos
    {
        get
        {
            float z = camera.WorldToScreenPoint(transform.position).z;
            return camera.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
        }
    }

    private bool isClickedOn
    {
        get
        {
            Ray ray = camera.ScreenPointToRay(curScreenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform == transform;
            }
            return false;
        }
    }

    private void Awake()
    {
        camera = Camera.main;

        // Initialize input actions
        inputActions = new Ball.Ball_Input_Actions();

        // Enable actions
        inputActions.Ball.Enable();

        // Update screenPos whenever performed
        inputActions.Ball.screenPos.performed += ctx =>
        {
            curScreenPos = ctx.ReadValue<Vector2>();
        };

        // Start drag on press performed if clicked on object
        inputActions.Ball.press.performed += _ =>
        {
            if (isClickedOn) StartCoroutine(Drag());
        };

        // Stop dragging on press canceled
        inputActions.Ball.press.canceled += _ => { isDragging = false; };
    }

    private void OnEnable()
    {
        inputActions?.Enable();
    }

    private void OnDisable()
    {
        inputActions?.Disable();
    }

    private void Update()
    {
        // Read scroll input from scroll wheel (Vector2, Y component) and alt+mouse delta (float)
        float scrollWheelZ = inputActions.Ball.scrollZ.ReadValue<Vector2>().y;
        float altScrollZ = inputActions.Ball.altScrollZ.ReadValue<float>();

        // Combine scroll inputs, scaled by their speeds
        float combinedZ = scrollWheelZ * scrollSpeed + altScrollZ * altScrollSpeed;

        if (Mathf.Abs(combinedZ) > 0.01f)
        {
            // Move ball forward or backward along camera's forward direction
            transform.position += camera.transform.forward * combinedZ * Time.deltaTime;
        }
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        Vector3 offset = transform.position - WorldPos;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        while (isDragging)
        {
            transform.position = WorldPos + offset;
            yield return null;
        }

        rb.useGravity = true;
    }
}
