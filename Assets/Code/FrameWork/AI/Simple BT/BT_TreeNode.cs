﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Status = BT_Behavior.Status;
using System;

[System.Serializable]
public class BT_TreeNode
{
    #region Fields

    public BT_TreeNode Parent;
    public List<BT_TreeNode> Children;
    public int ID;
    public AI_Blackboard ParametersBB;

    private BT_BBParameters behavior;

    #endregion

    #region Properties

    public BT_BBParameters Behavior
    {
        get { return behavior; }
        set { SetParameters(value); }
    }

    public bool HasChildren { get { return Children.Count > 0; } }
    public bool IsRoot { get { return Parent == null; } }

    #endregion

    #region Constructor

    public BT_TreeNode(BT_BBParameters behavior)
    {
        // Set Parameters on behavior
        Children = new List<BT_TreeNode>();
        ID = -1337;
        Parent = null;

        ParametersBB = new AI_Blackboard();
        SetParameters(behavior);
    }

    public BT_TreeNode(BT_BBParameters behavior, int id, BT_TreeNode parent, params BT_TreeNode[] childrenMem)
    {
       
        ID = id;
        Children = childrenMem.ToList();
        Parent = parent;

        ParametersBB = new AI_Blackboard();
        SetParameters(behavior);
        
    }


    private void SetParameters(BT_BBParameters behavior)
    {
        // Set behavior
        this.behavior = behavior;
        
        // Reset blackboard
        ParametersBB = new AI_Blackboard();
        // Call the SetnodeParameters virtual method
        // Sets the blackboard with default parameters
        behavior.SetNodeParameters(this);
    }

    #endregion

    /// <summary>
    /// Returns false if the class already exists and true if it had to be created
    /// </summary>
    public bool CheckAndSetClass<T>() where T: BT_BBParameters
    {
        if (Behavior.GetType() == typeof(T))
            return false;

        // Not the same type so reset the behavior
        T newBehavior = (T)Activator.CreateInstance(typeof(T));
        Behavior = newBehavior;
        return true;
    }

    public Status Tick(AI_Agent agent)
    {
        return Behavior.Tick(agent, ID);
    }
}