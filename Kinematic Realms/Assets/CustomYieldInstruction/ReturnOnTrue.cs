using System;
using UnityEngine;

public class ReturnOnTrue : CustomYieldInstruction
{
    Func<bool> booleanFunction;
    public override bool keepWaiting
    {
        get
        {
            
            return !booleanFunction();

        }
    }

    public ReturnOnTrue(Func<bool> booleanFunction)
    {
        this.booleanFunction = booleanFunction;
       
    }
}