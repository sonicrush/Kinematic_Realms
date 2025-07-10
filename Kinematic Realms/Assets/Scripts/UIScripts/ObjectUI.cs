using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{
    public bool attachToObject;
    public int yOffsetFromObject;
    public InputAction leftClick;
    [System.NonSerialized] public GameObject screenCanvas;
    [System.NonSerialized] public GameObject worldSpaceUI;
    private Toggle accelerationUIToggle;
    private Toggle accelerationVectorToggle;
    private GameObject accelerationUIDisplayObject;
    private AccelerationVector accelerationVectorComponent;
    private bool secondsElapsed;
    [System.NonSerialized] public bool isToggleObjectUIRunning;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject WorldSpaceUIPrefab = Resources.Load<GameObject>("Prefabs/ObjectUI");
        GameObject screenCanvasPrefab = Resources.Load<GameObject>("Prefabs/ScreenCanvas");

        screenCanvas = Instantiate(screenCanvasPrefab);
        worldSpaceUI = Instantiate(WorldSpaceUIPrefab);

        leftClick = InputSystem.actions.FindAction("UI/Click");

        if (attachToObject)
        {
            worldSpaceUI.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
            worldSpaceUI.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = worldSpaceUI.GetComponent<Transform>(), weight = 1 });
            worldSpaceUI.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(0, yOffsetFromObject, 0));
        }
    



        Toggle[] toggles = worldSpaceUI.GetComponentsInChildren<Toggle>(true);

        


        int accelerationUIToggleLocation = 0;
        int accelerationVectorToggleLocation = 1;
        accelerationUIToggle = toggles[accelerationUIToggleLocation];
        accelerationVectorToggle = toggles[accelerationVectorToggleLocation];

        accelerationUIToggle.onValueChanged.AddListener(ToggleAccelerationUIComponent);
        accelerationVectorToggle.onValueChanged.AddListener(ToggleAccelerationVectorComponent);
    }

    // Update is called once per frame
    void Update()
    {
        if (leftClick.WasPressedThisFrame())
        {
            OnMouseDown();
        }
    }
    void OnMouseDown()
    {
        print("Mouse down!");
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.rigidbody.gameObject == gameObject)
                StartCoroutine(toggleObjectUI());
        }

    }
    IEnumerator toggleObjectUI()
    {

        if (isToggleObjectUIRunning)
        {
            //this prevents from the function being called repetedly if the object is spam clicked
            yield break;
        }

        isToggleObjectUIRunning = true;
        secondsElapsed = false;

        StartCoroutine(elapseSeconds(0.2f));
        //This makes the function continue only when the mouse is up
        yield return new ReturnOnFalse(leftClick.IsPressed);

        //This checks to see if the elapseSeconds co-routine has finished. If so, do not display the UI.
        //This is to avoid showing the UI when the user's intention is to drag the object.
        //This was my solution to a kind of "If the user's mouse is still down after x seconds, do not display the ui. Otherwise, display the ui." 
        //I am proud of coming up with this solution.
        worldSpaceUI.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position;
        worldSpaceUI.GetComponent<Transform>().Translate(0, yOffsetFromObject+1, 0);
        if (!secondsElapsed)
        {
            worldSpaceUI.gameObject.SetActive(true);
        }
        isToggleObjectUIRunning = false;
        
    }
    IEnumerator elapseSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        secondsElapsed = true;
    }
    void ToggleAccelerationUIComponent(bool isChecked)
    {
        if (accelerationUIDisplayObject == null)
        {
            GameObject accelerationUIDisplayPrefab = Resources.Load<GameObject>("Prefabs/ScreenCanvasUI/AccelerationDisplay");
            accelerationUIDisplayObject = Instantiate(accelerationUIDisplayPrefab,screenCanvas.GetComponent<Transform>());
            
            accelerationUIDisplayObject.GetComponentInChildren<AccelerationUIDisplay>(true).referenceObject = gameObject;
        }
    
        accelerationUIDisplayObject.SetActive(isChecked);

    }

    void ToggleAccelerationVectorComponent(bool isChecked)
    {
    
        if (accelerationVectorComponent == null)
        {
            accelerationVectorComponent = gameObject.AddComponent<AccelerationVector>();
            return;
        }
        accelerationVectorComponent.vectorArrow.SetActive(isChecked);
    }
}
