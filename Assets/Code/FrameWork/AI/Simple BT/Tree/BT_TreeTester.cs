﻿using UnityEngine;
using System.Collections;
using Status = BT_Behavior.Status;
using fc = Framework.Collections;
using TC = BT_TreeConstructor;

public class BT_TreeTester 
{
    #region Test actions

    public void SetTestTree(AI_Agent agent)
    {
        BT_TreeNode f = TC.UDel(TC.failUpdate, "Fail");
        BT_TreeNode s = TC.UDel(TC.succesUpdate, "Succes");
        BT_TreeNode r = TC.UDel(TC.runningUpdate, "Running");
        BT_TreeNode b = TC.UDel(TC.pauseUpdate, "Pause");

        //Root = TC.sel(TC.seq(TC.sel(TC.seq(s, f), s), s), s);
        //RebuildTree();
        agent.CheckTreeVersion();
    }

    public void TestBTBasicCompontents(AI_Agent agent)
    {
        int errors = 0;

        #region Standard nodes

        BT_TreeNode f = TC.UDel(TC.failUpdate, "Fail");
        BT_TreeNode s = TC.UDel(TC.succesUpdate, "Succes");
        BT_TreeNode r = TC.UDel(TC.runningUpdate, "Running");
        BT_TreeNode b = TC.UDel(TC.pauseUpdate, "Pause");

        #endregion

        if ((int)agent["Depth"] != 0)
            errors++;

        //SetAgentDebug(true);

        #region Composits: TC.selector, Sequencer

        // Check the TC.selector
        errorCheck(TC.sel(f, f, r, s), Status.Running, ref errors, agent);
        errorCheck(TC.sel(f, f, f, f), Status.Failed, ref errors, agent);
        errorCheck(TC.sel(f, f, s, f), Status.Succes, ref errors, agent);

        // Check the sequencer
        errorCheck(TC.seq(s, s, r, f), Status.Running, ref errors, agent);
        errorCheck(TC.seq(s, s, r, s), Status.Running, ref errors, agent);
        errorCheck(TC.seq(f, f, f, f), Status.Failed, ref errors, agent);
        errorCheck(TC.seq(s, s, s, s), Status.Succes, ref errors, agent);

        #endregion

        #region Depth test

        // Generic depth test
        //TestDepth(agent);

        #endregion

        #region Decorators: TC.invert & alwaysFail

        // Check the TC.inverter
        errorCheck(TC.inv(s), Status.Failed, ref errors, agent);
        errorCheck(TC.inv(f), Status.Succes, ref errors, agent);
        errorCheck(TC.inv(r), Status.Running, ref errors, agent);

        // Check the alwaysFailed
        errorCheck(TC.fail(s), Status.Failed, ref errors, agent);
        errorCheck(TC.fail(f), Status.Failed, ref errors, agent);
        errorCheck(TC.fail(r), Status.Failed, ref errors, agent);

        #endregion

        //BT_TreeNode node = new BT_TreeNode(new BT_Decorator());

        #region ReActivate Soon

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

        errorCheck(TC.eqBB(p1, local, p2, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, 0), Status.Succes, ref errors, agent);

        agent[p2, local] = int3;
        errorCheck(TC.eqBB(p1, local, p2, local), Status.Failed, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, 1), Status.Failed, ref errors, agent);

        // cross global and local int check
        agent[p1, global] = int1;
        agent[p2, global] = int3;
        errorCheck(TC.eqBB(p1, local, p1, global), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, p2, global), Status.Failed, ref errors, agent);

        // string check
        agent[p1, local] = str1;
        agent[p2, local] = str2;

        errorCheck(TC.eqBB(p1, local, p2, local), Status.Succes, ref errors, agent);

        agent[p2, local] = int3;
        errorCheck(TC.eqBB(p1, local, p2, local), Status.Failed, ref errors, agent);

        // cross global and local int check
        agent[p1, global] = str1;
        agent[p2, global] = str3;
        errorCheck(TC.eqBB(p1, local, p1, global), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, p2, global), Status.Failed, ref errors, agent);

        // Vector3 check
        agent[p1, local] = v1;
        agent[p2, local] = v2;

        errorCheck(TC.eqBB(p1, local, p2, local), Status.Succes, ref errors, agent);

        agent[p2, local] = v3;
        errorCheck(TC.eqBB(p1, local, p2, local), Status.Failed, ref errors, agent);

        // cross global and local int check
        agent[p1, global] = v1;
        agent[p2, global] = v3;
        errorCheck(TC.eqBB(p1, local, p1, global), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, p2, global), Status.Failed, ref errors, agent);


        #endregion

        #region Action: TC.copy BB value

        // int TC.copy
        // Check first
        agent[p1, local] = int1;
        agent[p2, local] = int2;

        errorCheck(TC.eqBB(p1, local, p2, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, 0), Status.Succes, ref errors, agent);

        // now TC.copy in 3
        errorCheck(TC.copy(p1, local, 3), Status.Succes, ref errors, agent);
        // Check if it went allright
        errorCheck(TC.eqBB(p1, local, p2, local), Status.Failed, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, 3), Status.Succes, ref errors, agent);

        // now TC.copy from p1 to p2
        errorCheck(TC.copy(p2, local, p1, local), Status.Succes, ref errors, agent);

        // Check
        errorCheck(TC.eqBB(p1, local, p2, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p1, local, 3), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(p2, local, 3), Status.Succes, ref errors, agent);

        #endregion

        #region Action: Queue push, pop, checkSize
        string queueP1 = "TestQueue";
        string qCompare = "TestQueueComparer";

        // Create queue and populate
        fc.Queue<int> queue1 = new fc.Queue<int>();
        queue1.Add(1);
        queue1.Add(2);
        queue1.Add(3);

        // Populate the board
        agent[queueP1, local] = queue1;
        agent[qCompare, local] = -1;

        // Check size
        errorCheck(TC.qSize(queueP1, local, 3), Status.Succes, ref errors, agent);

        // Check push
        errorCheck(TC.qPush(queueP1, local, 4), Status.Succes, ref errors, agent);
        errorCheck(TC.qSize(queueP1, local, 4), Status.Succes, ref errors, agent);

        // Check pop
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        errorCheck(TC.qSize(queueP1, local, 3), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(qCompare, local, 1), Status.Succes, ref errors, agent);
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(qCompare, local, 2), Status.Succes, ref errors, agent);
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(qCompare, local, 3), Status.Succes, ref errors, agent);
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        errorCheck(TC.eqBB(qCompare, local, 4), Status.Succes, ref errors, agent);
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        //Debug.Log(agent[qCompare, local]);
        errorCheck(TC.qPop(queueP1, local, qCompare, local), Status.Succes, ref errors, agent);
        errorCheck(TC.qSize(queueP1, local, 0), Status.Succes, ref errors, agent);

        #region Test stuff
        //Queue<int> q = new Queue<int>();
        //q.Enqueue(6);
        //q.Enqueue(4);
        //q.Enqueue(2);

        //BT_QueuePop whatev = new BT_QueuePop("test", local, "test2", local);
        ////int testooh = (int)whatev.GetFromGenericQueue(q);
        //Debug.Log((int)whatev.GetFromGenericQueue(q));
        //Debug.Log((int)whatev.GetFromGenericQueue(q));
        //Debug.Log((int)whatev.GetFromGenericQueue(q));

        //List<int> test = new List<int>() { 1, 2, 3 };
        //IList list = (IList)test;
        //Debug.Log(list[0]);


        //fc.Stack<int> stack1 = new fc.Stack<int>();
        //fc.Stack<string> stack2 = new fc.Stack<string>();
        //stack1.Add(1);
        //stack1.Add(2);
        //stack1.Add(3);

        //Debug.Log(stack1.GetType().GetGenericTypeDefinition() + " - " + stack1.Get() + " " + stack1.Get() + " " + stack1.Get() + " " + stack1.Get());
        //fc.Queue<int> queue1 = new fc.Queue<int>();
        //queue1.Add(1);
        //queue1.Add(2);
        //queue1.Add(3);

        //fc.IQueue queueI = queue1;
        //Debug.Log(queue1.GetType().GetGenericTypeDefinition() == typeof(fc.Queue<>));
        //Debug.Log(queue1.GetType().GetGenericTypeDefinition());// + " - " + queue1.Get() + " " + queue1.Get() + " " + queue1.Get() + " " + queue1.Get());
        //Debug.Log(queueI.GetType().GetGenericTypeDefinition() + " - " + queueI.Get() + " " + queueI.Get() + " " + queueI.Get() + " " + queueI.Get());
        #endregion

        #endregion

        #endregion

        //Debug.Log("Depth" + (int)agent["Depth"]);
        if ((int)agent["Depth"] != 0)
            errors++;

        if (errors != 0)
            Debug.Log("Behavior Tree test FAILED - " + errors + " Errors.");
        else
            Debug.Log("Behavior Tree test SUCCES - 0 Errors.");
    }

    #region Helpers

    private void SetAgent(AI_Agent.BlackBoard access, string p1, string p2, object obj1, object obj2, AI_Agent agent)
    {
        agent[p1, access] = obj1;
        agent[p2, access] = obj2;
    }

    public void TestDepth(AI_Agent agent)
    {
        BT_TreeNode f = TC.UDel(TC.failUpdate, "Fail");
        BT_TreeNode s = TC.UDel(TC.succesUpdate, "Succes");
        BT_TreeNode r = TC.UDel(TC.runningUpdate, "Running");
        BT_TreeNode b = TC.UDel(TC.pauseUpdate, "Pause");

        BT_TreeNode tree = TC.sel(TC.sel(TC.sel(TC.sel(s, s), s), s), s);

        //RebuildTree(tree);
        agent.CheckTreeVersion();

        tree.Tick(agent);
    }

    private void errorCheck(BT_TreeNode root, Status returnStatus, ref int errors, AI_Agent agent)
    {
        //RebuildTree(root);
        agent.CheckTreeVersion();
        Status beh = root.Tick(agent);

        // Check if it is the correct return type and if its not TC.invalid
        if (beh != returnStatus)
            errors++;
        if (beh == Status.Invalid)
            errors++;
    }

    #endregion

    #endregion
}
