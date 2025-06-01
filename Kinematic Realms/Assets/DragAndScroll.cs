using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class DragAndScroll : MonoBehaviour
{
    private Camera cam; // Stores the reference to the main camera
    private bool isDragging = false; // Keeps track of whether we're dragging the object
    private Vector3 offset; // Offset from the mouse hit point to the object position
    private Plane dragPlane; // The imaginary plane used to calculate drag position

    public float scrollSpeed = 5f; // Speed multiplier for scroll movement
    public float altMouseSensitivity = 0.5f; // Sensitivity for ALT + mouse movement

    private void Start()
    {
        cam = Camera.main; // Get the Main Camera
    }

    private void Update()
    {
        HandleDrag(); // Controls dragging on the XY plane
        HandleScroll(); // Controls movement along Z axis
    }

    void HandleDrag()
    {
        // Start dragging when left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // Ray from camera to mouse
            RaycastHit hit;

            // Check if we clicked this object
            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isDragging = true;

                // Create a drag plane parallel to the XY plane, at the object's Z position
                dragPlane = new Plane(Vector3.forward, transform.position);
                float enter;
                if (dragPlane.Raycast(ray, out enter))
                {
                    // Get the point on the plane where the ray hits
                    offset = transform.position - ray.GetPoint(enter);
                }
            }
        }

        // Stop dragging when mouse is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // While dragging and ALT is not held
        if (isDragging && !Input.GetKey(KeyCode.LeftAlt))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float enter;

            if (dragPlane.Raycast(ray, out enter))
            {
                // Move object based on the hit point + offset, keeping Z unchanged
                Vector3 point = ray.GetPoint(enter);
                transform.position = new Vector3(point.x + offset.x, point.y + offset.y, transform.position.z);
            }
        }

    }

    void HandleScroll()
    {
        float scrollDelta = Input.mouseScrollDelta.y; // How much scroll wheel moved

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            // Simulate scroll with ALT + mouse movement
            float altDelta = Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y");
            scrollDelta += altDelta * altMouseSensitivity;
        }

        if (scrollDelta != 0f && isDragging)
        {
            // Move the object forward/backward (Z-axis) relative to world space
            transform.position += Vector3.forward * scrollDelta * scrollSpeed * Time.deltaTime;
        }
    }
}