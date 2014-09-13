﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D)),RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour 
{
    public float MaximumVelocity = 10;

    private bool facingLeft = true;

    private Transform tr;
    private Rigidbody2D rb;
    private Animator an;

    private string velXStr = "VelocityX";
    private string velYStr = "VelocityY";

    private Vector2 movementInput = new Vector2();

    // Use this for initialization
	void Start () 
    {
        tr = transform;
        rb = rigidbody2D;
        an = GetComponent<Animator>();
        rb.gravityScale = 0;
        rb.fixedAngle = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        rb.velocity = movementInput * MaximumVelocity;

        // Set animator
        an.SetFloat(velXStr, rb.velocity.x);
        an.SetFloat(velYStr, rb.velocity.y);

        CheckFlipBy(movementInput.x);
        HandleZDepth();
	}

    public void SetInput(float horizontalInput, float verticalInput)
    {
        movementInput.x = horizontalInput;
        movementInput.y = verticalInput;
        if(movementInput.magnitude>1)
            movementInput.Normalize();
    }

    private void HandleZDepth()
    {
        Vector3 pos = tr.position;
        pos.z = pos.y;
        tr.position = pos;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 localScale = tr.localScale;
        localScale.x = -localScale.x;
        tr.localScale = localScale;
    }

    /// <summary>
    /// dir positive for right
    /// dir negative for left
    /// </summary>
    /// <param name="dir"></param>
    private void CheckFlipBy(float dir)
    {
        if (dir == 0)
            return;
        dir = Mathf.Sign(dir);

        if (dir > 0 && facingLeft)
            Flip();
        else if (dir < 0 && !facingLeft)
            Flip();
    }
}
