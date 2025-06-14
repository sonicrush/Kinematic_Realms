using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class DragAndScroll : MonoBehaviour
{
    // Reference to the generated InputActions class (replace BallInputActions with your generated class name if different)
    private Ball.Ball_Input_Actions inputActions;

    private Vector3 curScreenPos;
    private new Camera camera;
    private bool isDragging;

    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private float altScrollSpeed = 3f;
    [SerializeField] private float dragSmoothing = 15f;

    private Rigidbody rb;
    private Vector3 dragTargetPos;
    private Vector3 dragOffset;
    private Vector3 lastPosition;

    private bool isScrollMoving = false;

    public Vector3 CurrentVelocity { get; private set; }

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
        rb = GetComponent<Rigidbody>();

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

    private void OnEnable() => inputActions?.Enable();
    private void OnDisable() => inputActions?.Disable();

    private void Update()
    {
        // Read scroll input from scroll wheel (Vector2, Y component) and alt+mouse delta (float)
        float scrollWheelZ = inputActions.Ball.scrollZ.ReadValue<Vector2>().y;
        float altScrollZ = inputActions.Ball.altScrollZ.ReadValue<float>();

        // Combine scroll inputs, scaled by their speeds
        float combinedZ = scrollWheelZ * scrollSpeed + altScrollZ * altScrollSpeed;

        isScrollMoving = Mathf.Abs(combinedZ) > 0.01f;

        if (isScrollMoving)
        {
            // Move ball forward or backward along camera's forward direction
            Vector3 movement = camera.transform.forward * combinedZ;
            rb.MovePosition(rb.position + movement * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        CurrentVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        dragOffset = transform.position - WorldPos;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        while (isDragging)
        {
            dragTargetPos = WorldPos + dragOffset;

            // Smoothly interpolate position toward target
            Vector3 newPosition = Vector3.Lerp(rb.position, dragTargetPos, Time.deltaTime * dragSmoothing);
            rb.MovePosition(newPosition);

            yield return new WaitForFixedUpdate(); // sync with physics
        }

        rb.useGravity = true;
    }
}
