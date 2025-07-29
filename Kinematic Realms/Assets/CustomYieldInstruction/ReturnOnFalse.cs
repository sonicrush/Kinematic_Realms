using System;
using UnityEngine;

public class ReturnOnFalse : CustomYieldInstruction
{
    Func<bool> booleanFunction;
    public override bool keepWaiting
    {
        get
        {
            
            return booleanFunction();

        }
    }

    public ReturnOnFalse(Func<bool> booleanFunction)
    {
        this.booleanFunction = booleanFunction;
    }
}