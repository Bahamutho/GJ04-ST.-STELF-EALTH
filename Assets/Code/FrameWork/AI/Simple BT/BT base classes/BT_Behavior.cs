﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Base class where Actions, Conditions, Composits: Sequencers and Selectors and Decorators are based on
/// </summary>
[System.Serializable]
public class BT_Behavior:ScriptableObject
{
    #region Class and enum
    public class NodeDescription
    {
        public enum BT_NodeType
        {
            Action,
            Composite,
            Decorator,
            Condition
        }

        public string Name = "NotSet";
        public string Description = "IAM DESCRIPTIONORS";
        public BT_NodeType Type = BT_NodeType.Action;

        

        // TODO Public node paramters
        // Seperate out to "Instance" specific - not needed
        //public AI_Blackboard NodeBlackBoard = new AI_Blackboard();

        #region Constructor

         public NodeDescription(BT_NodeType type = BT_NodeType.Action, string name = "NoNameSet", string description = "I need a description.")
        {
            Name = name;
            Type = type;
            Description = description;
            
        }

        #endregion
    }

    public enum Status
    {
        Running,
        Succes,
        Failed,
        Invalid
    }

    #endregion

    #region fields

    public NodeDescription Description = new NodeDescription();
    protected int CurrentID;
    protected AI_Agent currentAgent;

    private string debugTree = "DebugTree";
    

    #endregion

    #region virtual functions

    protected virtual Status update(AI_Agent agent,int id) { return Status.Invalid; }
    protected virtual void onInitialize(AI_Agent agent, int id) { }
    protected virtual void onTerminate(AI_Agent agent, int id, Status status) { }

    #endregion

    #region Tick function

    public Status Tick(AI_Agent agent, int id)
    {
        CurrentID = id;
        currentAgent = agent;

        // Start if not yet initialized
        if (agent[id] == Status.Invalid)
            onInitialize(agent,id);

        // Update the behaviour
        // Save the state to the agent
        Status status = update(agent, id);
        agent[id] = status;

        // Stop if not still running
        if (status != Status.Running)
            onTerminate(agent, id, status);

        if (agent != null && agent.LocalBlackboard.GetObject<bool>(debugTree, true))
        {
            Debug.Log(Description.Type + " - " + Description.Name + " - " + status + " - " + agent["Depth"]);
        }

        // Save the last state
        // Move to parameterized bb or something similar
        return status;
    }
    #endregion
}
