﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class HandleSpriteOrdering : MonoBehaviour 
{
    List<SpriteRenderer> renderers;
    Transform tr;
    public bool Static;
	// Use this for initialization
	void Start () 
    {
        renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        tr = transform;
        if (!Static)
            RedoSortingOrder();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Static)
            return;
        RedoSortingOrder();
	}

    private void RedoSortingOrder()
    {
        foreach (SpriteRenderer rndr in renderers)
        {
            rndr.sortingOrder = (int)(tr.position.y * -10);
        }
    }
}