using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUI : MonoBehaviour
{

    public Canvas worldSpaceUI;
    public Toggle accelerationUIToggle;
    public Toggle accelerationVectorToggle;
    public AccelerationUIDisplay accelerationUIDisplayComponent;
    bool secondsElapsed;
    bool isToggleObjectUIRunning;
    // public AccelerationVector accelerationVectorComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //worldSpaceUI = Instantiate(WorldSpaceUIPrefab, transformation parent as gameobject.getComponent<Transform>())

        Toggle[] toggles = worldSpaceUI.GetComponentsInChildren<Toggle>();
        accelerationUIToggle = toggles[0];
        accelerationVectorToggle = toggles[1];
        accelerationUIToggle.onValueChanged.AddListener(ToggleAccelerationUIComponent);
        worldSpaceUI.gameObject.SetActive(false);
        // accelerationVectorToggle.onValueChanged.AddListener(ToggleAccelerationVectorComponent);



    }

    // Update is called once per frame
    void Update()
    {
    
    }
    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

        StartCoroutine(elapseSeconds(0.1f));
        //This makes the function continue only when the mouse is up
        yield return new ReturnOnMouseUp();

        //This checks to see if the elapseSeconds co-routine has finished. If so, do not display the UI.
        //This is to avoid showing the UI when the user's intention is to drag the object.
        //This was my solution to a kind of "If the user's mouse is still down after x seconds, do not display the ui. Otherwise, display the ui." 
        //I am proud of coming up with this solution.
        if (!secondsElapsed)
            worldSpaceUI.gameObject.SetActive(true);
        secondsElapsed = false;
        
    }
    IEnumerator elapseSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        secondsElapsed = true;
    }
    void ToggleAccelerationUIComponent(bool isChecked)
    {
        if (isChecked)
        {
            if (accelerationUIDisplayComponent == null)
            {
                accelerationUIDisplayComponent = gameObject.AddComponent<AccelerationUIDisplay>();
                return;
            }
            accelerationUIDisplayComponent.enabled = true;
            return;
        }
        accelerationUIDisplayComponent.enabled = false;
    }
    
    // void ToggleAccelerationVectorComponent(bool isChecked)
    // {
    //     if (isChecked)
    //     {
    //         if (accelerationVectorComponent == null)
    //         {
    //             accelerationVectorComponent = gameObject.AddComponent<AccelerationVector>();
    //             return;
    //         }
    //         accelerationVectorComponent.enabled = true;
    //         return;
    //     }
    //     accelerationVectorComponent.enabled = false;
    // }
}
