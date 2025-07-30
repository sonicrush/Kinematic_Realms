using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{
    public bool attachToObject;
    public int xOffsetFromObject;
    public int yOffsetFromObject;
    public int zOffsetFromObject;

    [System.NonSerialized] public GameObject screenCanvas;
    [System.NonSerialized] public GameObject worldSpaceUI;

    private GameObject accelerationUIDisplayObject;
    public AccelerationVector accelerationVectorComponent;

    public velocityVector velocityVectorComponent; 

    private InputAction leftClick;
    private bool secondsElapsed;
    private bool isToggleObjectUIRunning;

    //To add your own UI, modify the Prefab and create a toggle child object under image, then implement the toggle in this script.
    //Use existing implementations as examples.

    void Start()
    {
        AssignScreenCanvas();
        CreateWorldspaceUI();

        AssignVariables();
        AssignUIToggles();
    }

    private void AssignVariables()
    {
        leftClick = InputSystem.actions.FindAction("UI/Click");
        //Null cases for components and their creation should be handled in their respective toggle methods.
        accelerationVectorComponent = gameObject.GetComponent<AccelerationVector>();
        velocityVectorComponent = gameObject.GetComponent<velocityVector>();

    }

    private void AssignUIToggles()
    {
        Toggle[] toggles = worldSpaceUI.GetComponentsInChildren<Toggle>(true);
        //Location depends on the order of children within the Image object.
        int accelerationUIToggleLocation = 0;
        int accelerationVectorToggleLocation = 1;
        int velocityVectorToggleLocation = 2;

        Toggle accelerationUIToggle = toggles[accelerationUIToggleLocation];
        Toggle accelerationVectorToggle = toggles[accelerationVectorToggleLocation];
        Toggle velocityVectorToggle = toggles[velocityVectorToggleLocation];

        //Toggle methods should handle null component cases.
        accelerationUIToggle.onValueChanged.AddListener(ToggleAccelerationUIComponent);
        accelerationVectorToggle.onValueChanged.AddListener(ToggleAccelerationVectorComponent);
        velocityVectorToggle.onValueChanged.AddListener(ToggleVelocityVectorComponent);
    }

    //UI Toggle Methods
    public void ToggleAccelerationUIComponent(bool isChecked)
    {
        if (accelerationUIDisplayObject == null)
        {
            GameObject accelerationUIDisplayPrefab = Resources.Load<GameObject>("Prefabs/ScreenCanvasUI/AccelerationDisplay");
            accelerationUIDisplayObject = Instantiate(accelerationUIDisplayPrefab, screenCanvas.GetComponent<Transform>());

            accelerationUIDisplayObject.GetComponentInChildren<AccelerationUIDisplay>(true).referenceObject = gameObject;
        }

        accelerationUIDisplayObject.SetActive(isChecked);

    }

    public void ToggleAccelerationVectorComponent(bool isChecked)
    {
        if (accelerationVectorComponent == null)
        {
            accelerationVectorComponent = gameObject.AddComponent<AccelerationVector>();
            return;
        }
        accelerationVectorComponent.vectorArrow.SetActive(isChecked);
    }
    public void ToggleVelocityVectorComponent(bool isChecked)
    {
        print("TOGGLED");
        if (velocityVectorComponent == null)
        {
            velocityVectorComponent = gameObject.AddComponent<velocityVector>();
            return;
        }
        velocityVectorComponent.vectorArrow.SetActive(isChecked);
    }









    //ObjectUI Methods (No need to look past here to implement new toggles)
    void Update()
    {
        if (leftClick.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null && hit.rigidbody.gameObject == gameObject)
                    StartCoroutine(toggleObjectUI());
            }
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
        //This was my solution to "If the user's mouse is still down after x seconds, do not display the ui." 
        //I am proud of coming up with this solution.

        if (!secondsElapsed)
        {
            worldSpaceUI.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position;
            worldSpaceUI.GetComponent<Transform>().Translate(xOffsetFromObject, yOffsetFromObject + 1, zOffsetFromObject);
            worldSpaceUI.gameObject.SetActive(true);
        }
        isToggleObjectUIRunning = false;
        
    }
    IEnumerator elapseSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        secondsElapsed = true;
    }



    //Start Methods
    private void CreateWorldspaceUI()
    {
        GameObject WorldSpaceUIPrefab = Resources.Load<GameObject>("Prefabs/ObjectUI");
        worldSpaceUI = Instantiate(WorldSpaceUIPrefab);
        if (attachToObject)
        {
            worldSpaceUI.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = gameObject.GetComponent<Transform>(), weight = 1 });
            worldSpaceUI.GetComponent<ParentConstraint>().AddSource(new ConstraintSource { sourceTransform = worldSpaceUI.GetComponent<Transform>(), weight = 1 });
            worldSpaceUI.GetComponent<ParentConstraint>().SetTranslationOffset(1, new Vector3(xOffsetFromObject, yOffsetFromObject, zOffsetFromObject));
        }
    }

    private void AssignScreenCanvas()
    {
        //There should always ONLY be one ScreenCanvas.
        screenCanvas = GameObject.FindGameObjectWithTag("Screen Canvas");
        if (screenCanvas == null)
        {
            GameObject screenCanvasPrefab = Resources.Load<GameObject>("Prefabs/ScreenCanvas");
            screenCanvas = Instantiate(screenCanvasPrefab);
        }
        else
        {
            AccelerationUIDisplay uiDisplayComponent = screenCanvas.GetComponent<AccelerationUIDisplay>();
            if(uiDisplayComponent != null)
            accelerationUIDisplayObject = uiDisplayComponent.gameObject;
        }
       
    }
}