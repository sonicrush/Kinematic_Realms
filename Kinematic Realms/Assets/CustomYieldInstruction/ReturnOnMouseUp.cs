using UnityEngine;

public class ReturnOnMouseUp : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return !Input.GetMouseButtonDown(1);
        }
    }

    public ReturnOnMouseUp()
    {
        
    }
}