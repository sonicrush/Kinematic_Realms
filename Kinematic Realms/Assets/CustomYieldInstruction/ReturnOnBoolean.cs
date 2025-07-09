using UnityEngine;
using UnityEngine.InputSystem;

public class ReturnOnBoolean : CustomYieldInstruction
{
    bool passedBool;
    public override bool keepWaiting
    {
        get
        {
            return passedBool;
        }
    }

    public ReturnOnBoolean(bool passedBool)
    {
        passedBool = this.passedBool;
    }
}