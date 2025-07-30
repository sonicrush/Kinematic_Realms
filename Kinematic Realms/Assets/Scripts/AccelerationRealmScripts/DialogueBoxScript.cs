using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Rendering.DebugUI.Table;

public class DialogueBoxScript : MonoBehaviour
{
    public RectTransform boxRect;
    private float originalWidth;
    private float originalHeight;

    public GameObject nextObject;
    public Button nextButton;
    public TextMeshProUGUI nextText;
    public RectTransform nextRect;
    private float originalNextWidth;
    private float originalNextHeight;
    
    public GameObject previousObject;
    public Button previousButton;
    public TextMeshProUGUI previousText;
    public RectTransform previousRect;
    private float originalPreviousWidth;
    private float originalPreviousHeight;

    public GameObject PlayObject;
    public Button PlayButton;

    public TextMeshProUGUI dialogueText;
    public RectTransform dialogueRect;
    private float originalDialogueWidth;
    private float originalDialogueHeight;

    private bool secondsElapsed;
    private bool applyingForce;

    [NonSerialized] public GameObject Cube;
    [NonSerialized] public Rigidbody cubeRigidBody;
    [NonSerialized] public Transform cubeTransform;

    public PhysicsMaterial frictionless;
    public PhysicsMaterial grassPhysicsMaterial;
    public Material grassMaterial;
    public Material litMaterial;

    public GameObject quad;
    public MeshCollider quadMeshCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cube = GameObject.FindGameObjectWithTag("Cube");
        cubeRigidBody = Cube.GetComponent<Rigidbody>();
        cubeTransform = Cube.transform;

        boxRect = gameObject.GetComponent<RectTransform>();
        dialogueRect = dialogueText.gameObject.GetComponent<RectTransform>();
        nextRect = nextButton.gameObject.GetComponent<RectTransform>();
        previousRect = previousButton.gameObject.gameObject.GetComponent<RectTransform>();
        quadMeshCollider = quad.GetComponent<MeshCollider>();

        originalWidth = boxRect.rect.width;
        originalHeight = boxRect.rect.height;
        originalDialogueWidth = dialogueRect.rect.width;
        originalDialogueHeight = dialogueRect.rect.height;
        originalNextHeight = nextRect.rect.height;
        originalNextWidth = nextRect.rect.width;
        originalPreviousHeight = previousRect.rect.height;
        originalPreviousWidth = previousRect.rect.width;



        EventZero();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EventZero()
    {
        Time.timeScale = 0f;
        HidePlay();
        HidePrevious();
        nextButton.onClick.AddListener(EventOne);
        dialogueText.text = "Acceleration occurs whenever an objects speed up or slows down.\nLet's observe a few cases of that happening here.";
    
    }
    void EventOne()
    {
        RemoveButtonListeners();
        ShowPrevious();
        previousButton.onClick.AddListener(EventZero);
        nextButton.onClick.AddListener(EventTwo);
        dialogueText.text = "A orange vector will show the direction of acceleration.\r\nNotice how from rest, this ball begins to go faster and faster.\r\n(Press play to see!)";

        ShowPlay();
        PlayButton.onClick.AddListener(PhysicsEventOne);
        previousButton.onClick.AddListener(HidePlay);



    }
    void EventTwo() 
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventOne);
        nextButton.onClick.AddListener(EventThree);
        PlayButton.onClick.AddListener(PhysicsEventTwo);
        dialogueText.text = "Now, this next one starts with some velocity but experiences negative acceleration.";

    }
    void EventThree()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventTwo);
        nextButton.onClick.AddListener(EventFour);
        AddFromOriginalDialogueSize(100, 0);
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
        });
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
        });
        PlayButton.onClick.AddListener(PhysicsEventThree);
        dialogueText.text = "What you just observed is the effect of acceleration.\r\nAcceleration is defined as the change in velocity over time.\r\n\nLet's repeat the first example, but now with a velocity vector in red.";
        

    }
    void EventFour()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventThree);
        nextButton.onClick.AddListener(EventFive);
        PlayButton.onClick.AddListener(PhysicsEventFour);
        dialogueText.text = "Same thing, but with the second example.";
        nextButton.onClick.AddListener(HidePlay);
    }
    void EventFive()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventFour);
        nextButton.onClick.AddListener(EventSix);
        dialogueText.text = "Acceleration is what causes velocity to grow or diminish.\r\nOne formula for acceleration is\n Velocityfinal - Velocityinitial / seconds\n or in shorter terms, Vf - Vi / s\r\n\nThe resulting unit from this calculation is meters/seconds ^2, or more commonly, m/s^2 \r\n";
        AddFromOriginalDialogueSize(200, 0);
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
        });
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
            ShowPlay();
        });
    }
    void EventSix()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventFive);
        nextButton.onClick.AddListener(EventSeven);
        dialogueText.text = "Now let's observe some velocity graphs.\n What do you think is the acceleration if this is the velocity graph?";
        nextText.text = "Show the answer!";
        AddFromOriginalNextSize(10, 20);
        //instantiateGraph();
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();

        });
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();
        });


    }
    void EventSeven()
    {
        dialogueText.text = "The acceleration of this object is 0. Since velocity does not change over time, there is no acceleration.";
        nextText.text = "Next";
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventSix);
        nextButton.onClick.AddListener(EventEight);

    }
    void EventEight()
    {
        dialogueText.text = "What about this one's acceleration?";
        //instantiateGraph();
        nextText.text = "Show the answer!";
        AddFromOriginalNextSize(10, 20);
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventSeven);
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();

        });
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();

        });
        nextButton.onClick.AddListener(EventNine);
        

    }
    void EventNine()
    {
        nextText.text = "Next";
        dialogueText.text = "The acceleration is 1 m/s^2, as every second, the velocity increases by 1 m/s every second.";
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventEight);
        nextButton.onClick.AddListener(Event10);

    }
    void Event10()
    {
        dialogueText.text = "Lastly, what about this one?";
        nextText.text = "Show the answer!";
        RemoveButtonListeners();
        previousButton.onClick.AddListener(EventNine);
        nextButton.onClick.AddListener(Event11);
        AddFromOriginalNextSize(10, 20);
        //instantiateGraph();
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();

        });
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalNextSize(0, 0);
            nextText.text = "Next";
            //destroyGraph();

        });

    }
    void Event11()
    {
        dialogueText.text = "The acceleration is -2 m/s^2, as velocity decreases 2 m/s every second.";
        nextText.text = "Next";
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event10);
        nextButton.onClick.AddListener(Event12);

    }
    void Event12()
    {
        AddFromOriginalDialogueSize(160, 0);
        dialogueText.text = "Acceleration can be seen occurring naturally in many ways. One of these is Gravity. Gravity <b>constantly</b> pulls all objects on earth towards it at approximately 9.8 m/s^2.\r\n\nLet's observe that with this display as this ball falls.";
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event11);
        nextButton.onClick.AddListener(Event13);
        previousButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
        });
        ShowPlay();
        PlayButton.onClick.AddListener(PhysicsEventFive);

    }
    void Event13()
    {
        dialogueText.text = "One more example we'll see is friction. When an object rubs against another, it causes the moving object(s) to slow down. Let's observe this box moving on grass.\r\nNotice how it had acceleration opposing it's direction of movement.";
        RemoveButtonListeners();
        ShowPlay();
        previousButton.onClick.AddListener(Event12);
        nextButton.onClick.AddListener(Event14);
        AddFromOriginalDialogueSize(150, 0);
        nextButton.onClick.AddListener(() =>
        {
            AddFromOriginalDialogueSize(0, 0);
            HidePlay();
        });
        PlayButton.onClick.AddListener(PhysicsEventSix);
        

    }
    void Event14()
    {
        dialogueText.text = "You finished! You may now exit.";
        RemoveButtonListeners();
        HideNext();
        previousButton.onClick.AddListener(Event13);
        previousButton.onClick.AddListener(ShowNext);
        
        

    }
    void Event15()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event14);
        nextButton.onClick.AddListener(Event16);

    }
    void Event16()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event15);
        nextButton.onClick.AddListener(Event17);

    }
    void Event17()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event16);
        nextButton.onClick.AddListener(Event18);

    }
    void Event18()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event17);
        nextButton.onClick.AddListener(Event19);

    }
    void Event19()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event18);
        nextButton.onClick.AddListener(Event20);

    }
    void Event20()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event19);
        nextButton.onClick.AddListener(EventFinal);

    }
    void EventFinal()
    {
        RemoveButtonListeners();
        previousButton.onClick.AddListener(Event20);
        previousButton.onClick.AddListener(ShowNext);
        HideNext();

    }
    void HideNext()
    {
        nextObject.SetActive(false);
    }
    void ShowNext()
    {
        nextObject.SetActive(true);
    }
    void HidePrevious()
    {
        previousObject.SetActive(false);
    }
    void ShowPrevious() { previousObject.SetActive(true); }
    void HidePlay() { PlayObject.SetActive(false); }
    void ShowPlay() { PlayObject.SetActive(true); }

    void RemoveButtonListeners()
    {
        nextButton.onClick.RemoveAllListeners();
        previousButton.onClick.RemoveAllListeners();
        PlayButton.onClick.RemoveAllListeners();
    }
    void AddFromOriginalDialogueSize(float width, float height)
    {
        boxRect.sizeDelta = new Vector2(originalWidth + width, originalHeight + height);
        dialogueRect.sizeDelta = new Vector2(originalDialogueWidth + width, originalDialogueHeight);
    }
    void AddFromOriginalNextSize(float width, float height)
    {
        nextRect.sizeDelta = new Vector2(originalNextWidth + width, originalNextHeight + height);
    }
    void AddFromOriginalPreviousSize(float width, float height)
    {
        previousRect.sizeDelta = new Vector2(originalPreviousWidth + width, originalPreviousHeight + height);
    }


    void PhysicsEventOne()
    {
        if (!applyingForce)
        {
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-7.33f, 0.58f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            
            StartCoroutine(elapseSeconds(2.4f));
            
            StartCoroutine(ApplyForce(0.3f));
        }
        
    }
    void PhysicsEventTwo()
    {
        if (!applyingForce) 
        {
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-7.33f, 0.58f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            cubeRigidBody.linearVelocity = new Vector3(12, 0, 0);

            StartCoroutine(elapseSeconds(2.4f));

            StartCoroutine(ApplyForce(-0.3f));
        }
    }
    void PhysicsEventThree()
    {
        if (!applyingForce) 
        { 
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-7.33f, 0.58f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            //Cube.GetComponent<ObjectUI>().ToggleVelocityVectorComponent(true);

            StartCoroutine(elapseSeconds(2.4f));

            StartCoroutine(ApplyForce(0.3f));
        }
    }
    void PhysicsEventFour()
    { if (!applyingForce)
        {
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-7.33f, 0.58f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            //Cube.GetComponent<ObjectUI>().ToggleVelocityVectorComponent(true);
            cubeRigidBody.linearVelocity = new Vector3(12, 0, 0);
            
            StartCoroutine(elapseSeconds(2.4f));
           
            StartCoroutine(ApplyForce(-0.3f));
        }
        
    }
    void PhysicsEventFive()
    {
        if (!applyingForce) 
        { 
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-4.56f, 5.48f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationUIComponent(true);
            //Cube.GetComponent<ObjectUI>().ToggleVelocityVectorComponent(true);


            StartCoroutine(elapseSeconds(2.4f));

            StartCoroutine(ApplyForce(0.0f));
        }

    }
    void PhysicsEventSix()
    {
        if (!applyingForce)
        {
            Time.timeScale = 1.0f;
            cubeTransform.SetPositionAndRotation(new Vector3(-7.33f, 0.58f, -10.78f), Quaternion.identity);
            Cube.GetComponent<ObjectUI>().ToggleAccelerationVectorComponent(true);
            //Cube.GetComponent<ObjectUI>().ToggleVelocityVectorComponent(true);
            List<Material> materials = new List<Material> { grassMaterial };
            quad.GetComponent<MeshRenderer>().SetMaterials(materials);
            quadMeshCollider.material = grassPhysicsMaterial;
            cubeRigidBody.linearVelocity = new Vector3(12, 0, 0);
            
            StartCoroutine(elapseSeconds(2.4f));
            
            StartCoroutine(ApplyForceEventSix(0f));
        }

    }

    IEnumerator ApplyForce(float force)
    {
        applyingForce = true;
        while (!secondsElapsed)
        {
            cubeRigidBody.AddForce(force, 0, 0, ForceMode.Acceleration);
            
            yield return null;
        }
        secondsElapsed = false;
        cubeRigidBody.linearVelocity = Vector3.zero;
        applyingForce = false;
        yield break;
            
       
    }
    IEnumerator elapseSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        secondsElapsed = true;
        Time.timeScale = 0f;

    }
    IEnumerator ApplyForceEventSix(float force)
    {
        applyingForce = true;
        while (!secondsElapsed)
        {
            cubeRigidBody.AddForce(force, 0, 0, ForceMode.Acceleration);

            yield return null;
        }
        secondsElapsed = false;
        cubeRigidBody.linearVelocity = Vector3.zero;
        applyingForce = false;
        quadMeshCollider.material = frictionless;
        List<Material> materials = new List<Material> { litMaterial };
        quad.GetComponent<MeshRenderer>().SetMaterials(materials);

        yield break;


    }
}
