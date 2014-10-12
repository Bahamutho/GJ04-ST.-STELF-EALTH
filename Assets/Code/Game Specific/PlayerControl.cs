﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour 
{
    public enum PlayerActions
    {
        PickPocket = 0

    }
    
    public ControlScheme ControlScheme;
    
    private CharacterController cc;
    private PickPocket pp;

	// Use this for initialization
	void Start () 
    {
        cc = gameObject.GetComponent<CharacterController>();
        pp = gameObject.GetComponent<PickPocket>();

        ControlScheme = ControlScheme.CreateScheme<PlayerActions>();
        ControlScheme.Actions[(int)PlayerActions.PickPocket].Keys.Add(ControlKey.PCKey(KeyCode.Space));
        ControlScheme.Actions[(int)PlayerActions.PickPocket].Keys.Add(ControlKey.XboxButton(XboxCtrlrInput.XboxButton.A));
	}
	
	// Update is called once per frame
	void Update () 
    {
        ControlScheme.Update();

        cc.SetInput(ControlScheme.Horizontal.Value(), ControlScheme.Vertical.Value());
        pp.SetInput(ControlScheme.Actions[(int)PlayerActions.PickPocket].IsPressed());
	}
}