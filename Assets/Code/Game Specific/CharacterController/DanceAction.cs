﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class DanceAction : ICharacterAction
{
    #region Fields

    public string DanceAnimationVar = "Dance";

    private bool finished;
    private AnimatorCollectionWrapper anim;

    #endregion

    #region Properties

    public bool Interuptable
    {
        get { return true; }
    }

    public bool CanInterupt
    {
        get { return false; }
    }

    public bool Finished
    {
        get { return finished; }
    }

    public bool CannotMoveDuringAction
    {
        get { return true; }
    }

    public string Name
    {
        get { return "Dance"; }
    }

    #endregion

    #region Start & Stop

    public void StartAction(AnimatorCollectionWrapper animator)
    {
        finished = false;
        anim = animator;

        animator.SetBool(DanceAnimationVar, true);
        // Set Dancing animation = true
    }

    public void StopAction()
    {
        finished = true;
        // Set Dancing animation = false
        anim.SetBool(DanceAnimationVar, false);
    }

    #endregion
}
