﻿using UnityEngine;
using System.Collections;

public interface ICharacterAction 
{
    bool Interuptable { get; }
    bool CanInterupt { get; }
    bool Finished { get; }
    bool CannotMoveDuringAction { get; }
    string Name { get; }


    /// <summary>
    /// Start the action (could start a coroutine)
    /// </summary>
    void StartAction(ICharacterController controller);

    /// <summary>
    /// Also possible to call if it is not interuptable (do an interuptable check if you dont want that to happen).
    /// </summary>
    void StopAction();
}