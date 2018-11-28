namespace Urq
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents a attack data
    /// </summary>
    public abstract class IAttackData : ScriptableObject
    {
        abstract public IAttack GetAttack();
    }

    /// <summary>
    /// The actual attack for the data
    /// </summary>
    public abstract class IAttack
    {
        abstract public void ApplyAttack(Ship myShip, InputManagerHelper.ControlInputs inputs, Rigidbody2D myBody);
    }
}