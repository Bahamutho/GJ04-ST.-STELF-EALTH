﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Status = BT_Behavior.Status;

public class BT_BehaviorTree : MonoBehaviour
{
    #region Fields

    [Range(0.00000000001f,60)]
    public float UpdateFrequency = 10;

    BT_Behavior Tree;
    List<BT_UINode> Nodes;
    AI_Agent agent;

    #endregion

    #region Init & Update

    //// Use this for initialization
    //void Start () 
    //{
    //    //TestTreeFunctionality();
    //    if(Tree != null)
    //        StartCoroutine(updateCR());
    //}

    // Update loop
    public IEnumerator updateCR(AI_Agent agent)
    {
        if(Tree == null)
        {
            Debug.Log("BT_BehaviorTree not populated.");
             yield break;
        }
        while (Application.isPlaying)
        {
            Tree.Tick(agent);

            yield return new WaitForSeconds(1.0f/UpdateFrequency);
        }
    }

    #endregion

    #region Test actions

    public void TestBTBasicCompontents()
    {
        int errors = 0;

        #region Standard nodes

        BT_BehaviorDelegator f = getBeh(failUpdate, "Fail");
        BT_BehaviorDelegator s = getBeh(succesUpdate, "Succes");
        BT_BehaviorDelegator r = getBeh(runningUpdate, "Running");
        BT_BehaviorDelegator b = getBeh(pauseUpdate, "Pause");

        #endregion

        if ((int)agent["Depth"] != 0)
            errors++;

        //SetAgentDebug(true);

        #region Composits: Selector, Sequencer 

        // Check the selector
        errorCheck(sel(f, f, r, s), Status.Running, ref errors);
        errorCheck(sel(f, f, f, f), Status.Failed, ref errors);
        errorCheck(sel(f, f, s, f), Status.Succes, ref errors);

        // Check the sequencer
        errorCheck(seq(s, s, r, f), Status.Running, ref errors);
        errorCheck(seq(s, s, r, s), Status.Running, ref errors);
        errorCheck(seq(f, f, f, f), Status.Failed, ref errors);
        errorCheck(seq(s, s, s, s), Status.Succes, ref errors);

        #endregion

        #region Depth test

        // Generic depth test
        TestDepth();

        #endregion

        #region Decorators: Invert & alwaysFail

        // Check the inverter
        errorCheck(inv(s), Status.Failed, ref errors);
        errorCheck(inv(f), Status.Succes, ref errors);
        errorCheck(inv(r), Status.Running, ref errors);

        // Check the alwaysFailed
        errorCheck(fail(s), Status.Failed, ref errors);
        errorCheck(fail(f), Status.Failed, ref errors);
        errorCheck(fail(r), Status.Failed, ref errors);

        #endregion

        #region Condition: CheckEqualBBParameter

        // Things to compare
        int int1 = 0;
        int int2 = 0;
        int int3 = 1;
        string str1 = "bla";
        string str2 = "bla";
        string str3 = "notbla";
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.up;

        // BB params
        string p1 = "TestParam1";
        string p2 = "TestParam2";
        AI_Agent.BlackBoard local = AI_Agent.BlackBoard.local;
        AI_Agent.BlackBoard global = AI_Agent.BlackBoard.global;

        // Simple int check
        agent[p1, local] = int1;
        agent[p2, local] = int2;

        errorCheck(eqBB(p1, local, p2, local), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, 0), Status.Succes, ref errors);

        agent[p2, local] = int3;
        errorCheck(eqBB(p1, local, p2, local), Status.Failed, ref errors);
        errorCheck(eqBB(p1, local, 1), Status.Failed, ref errors);
        
        // cross global and local int check
        agent[p1, global] = int1;
        agent[p2, global] = int3;
        errorCheck(eqBB(p1, local, p1, global), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, p2, global), Status.Failed, ref errors);

        // string check
        agent[p1, local] = str1;
        agent[p2, local] = str2;

        errorCheck(eqBB(p1, local, p2, local), Status.Succes, ref errors);

        agent[p2, local] = int3;
        errorCheck(eqBB(p1, local, p2, local), Status.Failed, ref errors);

        // cross global and local int check
        agent[p1, global] = str1;
        agent[p2, global] = str3;
        errorCheck(eqBB(p1, local, p1, global), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, p2, global), Status.Failed, ref errors);

        // Vector3 check
        agent[p1, local] = v1;
        agent[p2, local] = v2;

        errorCheck(eqBB(p1, local, p2, local), Status.Succes, ref errors);

        agent[p2, local] = v3;
        errorCheck(eqBB(p1, local, p2, local), Status.Failed, ref errors);

        // cross global and local int check
        agent[p1, global] = v1;
        agent[p2, global] = v3;
        errorCheck(eqBB(p1, local, p1, global), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, p2, global), Status.Failed, ref errors);


        #endregion

        #region Action: Copy BB value

        // int copy
        // Check first
        agent[p1, local] = int1;
        agent[p2, local] = int2;

        errorCheck(eqBB(p1, local, p2, local), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, 0), Status.Succes, ref errors);

        // now copy in 3
        errorCheck(copy(p1, local, 3), Status.Succes, ref errors);
        // Check if it went allright
        errorCheck(eqBB(p1, local, p2, local), Status.Failed, ref errors);
        errorCheck(eqBB(p1, local, 3), Status.Succes, ref errors);

        // now copy from p1 to p2
        errorCheck(copy(p2, local, p1, local), Status.Succes, ref errors);
 
        // Check
        errorCheck(eqBB(p1, local, p2, local), Status.Succes, ref errors);
        errorCheck(eqBB(p1, local, 3), Status.Succes, ref errors);
        errorCheck(eqBB(p2, local, 3), Status.Succes, ref errors);

        #endregion

        if ((int)agent["Depth"] != 0)
            errors++;

        if (errors != 0)
            Debug.Log("Behavior Tree test FAILED - " + errors + " Errors." );
        else
            Debug.Log("Behavior Tree test SUCCES - 0 Errors.");
    }

    private void SetAgent(AI_Agent.BlackBoard access, string p1, string p2, object obj1, object obj2)
    {
        agent[p1, access] = obj1;
        agent[p2, access] = obj2;
    }

    public void TestDepth()
    {
        BT_BehaviorDelegator f = getBeh(failUpdate, "Fail");
        BT_BehaviorDelegator s = getBeh(succesUpdate, "Succes");
        BT_BehaviorDelegator r = getBeh(runningUpdate, "Running");
        BT_BehaviorDelegator b = getBeh(pauseUpdate, "Pause");

        BT_Behavior tree = sel(sel(sel(sel(s, s),s),s),s);

        tree.Tick(agent);
    }

    private void errorCheck(BT_Behavior behavior, Status returnStatus, ref int errors)
    {
        Status beh = behavior.Tick(agent);

        // Check if it is the correct return type and if its not invalid
        if (beh != returnStatus)
            errors++;
        if (beh == Status.Invalid)
            errors++;
    }

    #endregion

    #region BT Component Syntactic Sugar

    private BT_CopyBBParameter copy(string bbParameter1, AI_Agent.BlackBoard param1, string bbParameter2, AI_Agent.BlackBoard param2)
    {
        return new BT_CopyBBParameter(bbParameter1, param1, bbParameter2, param2);
    }

    private BT_CopyBBParameter copy(string bbParameter1, AI_Agent.BlackBoard param1, object obj)
    {
        return new BT_CopyBBParameter(bbParameter1, param1, obj);
    }

    private BT_CheckEqualBBParameter eqBB(string bbParameter1, AI_Agent.BlackBoard param1, string bbParameter2, AI_Agent.BlackBoard param2)
    {
        return new BT_CheckEqualBBParameter(bbParameter1, param1, bbParameter2, param2);
    }

    private BT_CheckEqualBBParameter eqBB(string bbParameter1, AI_Agent.BlackBoard param1, object obj)
    {
        return new BT_CheckEqualBBParameter(bbParameter1, param1, obj);
    }

    private BT_AlwayFail fail(BT_Behavior child)
    {
        return new BT_AlwayFail(child);
    }

    private BT_Inverter inv(BT_Behavior child)
    {
        return new BT_Inverter(child);
    }

    private BT_Selector sel(params BT_Behavior[] behaviors)
    {
        return new BT_Selector(behaviors);
    }

    private BT_Sequencer seq(params BT_Behavior[] behaviors)
    {
        return new BT_Sequencer(behaviors);
    }

    #endregion

    #region Delegator

    private BT_BehaviorDelegator getBeh(BT_BehaviorDelegator.UpdateDelegate del, string name)
    {
        BT_BehaviorDelegator b = new BT_BehaviorDelegator(BT_Behavior.NodeDescription.BT_NodeType.Action, del);
        b.Description.Name = name;
        return b;
    }

    

    private BT_Behavior.Status failUpdate(AI_Agent agent, BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Failed;
    }

    private BT_Behavior.Status succesUpdate(AI_Agent agent, BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Succes;
    }

    private BT_Behavior.Status runningUpdate(AI_Agent agent, BT_Behavior.NodeDescription node)
    {
        return BT_Behavior.Status.Running;
    }

    private BT_Behavior.Status pauseUpdate(AI_Agent agent, BT_Behavior.NodeDescription node)
    {
        int Depth = (int)agent["Depth"];
        Debug.Break();
        return Status.Succes;
    }

    #endregion

    #region Helpers

    private void SetAgentDebug(bool debug)
    {
        agent.LocalBlackboard.SetObject("DebugTree", debug);
    }

    public void SetAgent(AI_Agent agent)
    {
        this.agent = agent;

        // TODO Setup UI nodes

    }

    #endregion
}
