﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BT_BehaviorTree : MonoBehaviour 
{
    BT_Behavior Root;
    List<BT_UINode> Nodes;

	// Use this for initialization
	void Start () 
    {
        BT_BehaviorDelegator f = new BT_BehaviorDelegator(BT_Behavior.NodeDescription.BT_NodeType.Action, failUpdate);
        f.Description.Name = "Fail";
        BT_BehaviorDelegator s = new BT_BehaviorDelegator(BT_Behavior.NodeDescription.BT_NodeType.Action, succesUpdate);
        s.Description.Name = "Succes";
        BT_BehaviorDelegator r = new BT_BehaviorDelegator(BT_Behavior.NodeDescription.BT_NodeType.Action, runningUpdate);
        r.Description.Name = "Running";

        BT_Selector sel1 = new BT_Selector(f, r);
        BT_Sequencer seq1 = new BT_Sequencer(r, s);
        
        Root = new BT_Selector(sel1, seq1);

        Root.Tick();


	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    private BT_Behavior.Status failUpdate(BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Failed;
    }

    private BT_Behavior.Status succesUpdate(BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Succes;
    }

    private BT_Behavior.Status runningUpdate(BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Running;
    }
}
