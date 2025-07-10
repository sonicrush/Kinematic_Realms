using UnityEngine;

public class ToggleVGraph : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void ToggleGraph()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}