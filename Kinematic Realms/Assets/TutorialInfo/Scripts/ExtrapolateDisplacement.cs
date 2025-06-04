using UnityEngine;


public class ExtrapolateDisplacement : MonoBehaviour
{

    private Vector3 startPos;
    public Vector3 displacement;
    private bool logged = false;

 

    void Start()
    {
        startPos = gameObject.transform.position; // init the starting position
    }


    void Update()
    {

        if (gameObject.GetComponent<Rigidbody>().IsSleeping() && !logged)
        {

            displacement = transform.position - startPos; 
            Debug.Log("X: " + Mathf.Round(displacement.x) + "\nY: " + Mathf.Round(displacement.y) + "\nZ: " + Mathf.Round(displacement.z));
            logged = true; 
        }else if(!gameObject.GetComponent<Rigidbody>().IsSleeping() && logged)
        {
            logged = false; // checks if object starts moving again
        }
        



    }
}
