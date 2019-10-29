using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 5.0f;
    public float AirControl = 0.5f;
    public Vector2 FootSize = new Vector2(0.1f, 0.01f);
    public float JumpVelocity = 8.0f;
    public LayerMask GroundMask;
    public float ResetJumpDelay = 0.2f;
    public Transform sprite;

    public bool FaceRight
    {
        get
        {
            return faceRight;
        }
    }

    public bool Ground
    {
        get
        {
            return CheckIfGrounded();
        }
    }

    Rigidbody2D rb2D;
    Vector2 footTopLeftCorner;
    Vector2 footBottomRightCorner;
    public bool faceRight = true;
    bool jumped = false;

    // public methods
    public void Jump()
    {
        rb2D.velocity = rb2D.velocity + Vector2.up * JumpVelocity;
        jumped = true;
        CancelInvoke("ResetJumped");
        Invoke("ResetJumped", ResetJumpDelay);
    }

    public void Move(float input)
    {
        if (!Ground)
        {
            input = input * AirControl;
        }

        rb2D.velocity = new Vector2(input * Speed, rb2D.velocity.y);

        FlipIfNecessary(input);
    }

    void Start()
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void ResetJumped()
    {
        jumped = false;
    }

    bool CheckIfGrounded()
    {
        if (jumped)
        {
            return false;
        }

        footTopLeftCorner = new Vector2(transform.position.x - FootSize.x / 2, transform.position.y - FootSize.y / 2);
        footBottomRightCorner = new Vector2(transform.position.x + FootSize.x / 2, transform.position.y + FootSize.y / 2);

        Debug.DrawLine(footTopLeftCorner, footTopLeftCorner + Vector2.right * FootSize.x, Color.green);
        Debug.DrawLine(footTopLeftCorner, footTopLeftCorner - Vector2.down * FootSize.y, Color.green);
        Debug.DrawLine(footBottomRightCorner, footBottomRightCorner - Vector2.right * FootSize.x, Color.green);
        Debug.DrawLine(footBottomRightCorner, footBottomRightCorner + Vector2.down * FootSize.y, Color.green);

        return Physics2D.OverlapArea(footTopLeftCorner, footBottomRightCorner, GroundMask);
    }

    void FlipIfNecessary(float move)
    {
        if (move > 0 && !faceRight)
        {
            Flip();
        }
        else if (move < 0 && faceRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        faceRight = !faceRight;
    }

    private void Update()
    {
        Vector3 theScale = sprite.localScale;
        theScale.x = faceRight ? 1 :- 1;
        sprite.localScale = theScale;
    }
}