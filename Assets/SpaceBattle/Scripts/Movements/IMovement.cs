namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// All the stuff needed to be a movement
    /// </summary>
    public abstract class IMovement : ScriptableObject
    {
        abstract public void ApplyMovement(InputManagerHelper.ControlInputs inputs, Rigidbody2D myBody);       
    }
}