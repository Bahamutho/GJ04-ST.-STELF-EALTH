﻿using UnityEditor;
using UnityEngine;

using System;
using System.Reflection;
using System.Linq;
using System.Collections;

using nodeType = BT_Behavior.NodeDescription.BT_NodeType;
using NodeDescription = BT_Behavior.NodeDescription;
using System.Collections.Generic;

[CustomEditor(typeof(BT_UINode)),ExecuteInEditMode]
public class BT_UINodeEditor : EditorPlus
{
    private int selectedClass = 0;
    private int selectedField = 0;
    private nodeType lastType = nodeType.Action;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BT_UINode node = (BT_UINode)target;

        Type type = GetType(node.Type);
        
        if(lastType != node.Type)
        {
            selectedClass = 0;
            selectedField = 0;
        }
        lastType = node.Type;

        // Get all the classes from the assembly that inherent from the selected BT node type
        var q1 =     from t in Assembly.GetAssembly(typeof(BT_Behavior)).GetTypes()
                        where t.IsClass && (t.IsSubclassOf(type)||t == type)
                        select t;

        var q2 = from t in q1
                 select t.Name.ToString();


        List<string> classList = q2.ToList<string>();
        
        selectedClass = EditorGUILayout.Popup(selectedClass, classList.ToArray());
        
        var l1 = q1.ToList();
        if (GUILayout.Button("Replace node"))
            node.Node = (BT_Behavior)Activator.CreateInstance(l1[selectedClass]);


        //GetNodeDescription
        //var methods = l1[selectedClass].GetMethods(BindingFlags.Public | BindingFlags.Static);

        //var q5 = from m in methods
        //         where m.Name == "GetNodeDescription"
        //         select m;

        //MethodInfo mi = q5.FirstOrDefault();

        //NodeDescription descr = (NodeDescription)mi.Invoke(null, null);

        if (node == null)
            return;

        EditorGUILayout.LabelField("Name: ",node.Node.Description.Name);
        EditorGUILayout.LabelField(node.Node.Description.Description);
        //EditorGUILayout.LabelField("Name: ", node.Node.Description.Type.ToString());

        // Show all the fields from the selected class
        var fields = l1[selectedClass].GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);

        var q3 = from f in fields
                 select f.Name.ToString();

        List<string> fieldList = q3.ToList();

        var q4 = fields.Where(f => f.Name == "Description").FirstOrDefault();

        selectedField = EditorGUILayout.Popup(selectedField, fieldList.ToArray());
    }

    private Type GetType(nodeType bT_NodeType)
    {
        switch (bT_NodeType)
        {
            case nodeType.Action:
                return typeof(BT_Action);
            case nodeType.Condition:
                return typeof(BT_Condition);
            case nodeType.Decorator:
                return typeof(BT_Decorator);
            case nodeType.Selector:
                return typeof(BT_Selector);
            case nodeType.Sequencer:
                return typeof(BT_Sequencer);
        }
        return typeof(BT_Behavior);
    }
}
