using System.Collections;
using System.Collections.Generic;
using SWNetwork;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool moveToRight = true;

    [Header("Detection")]
    public float leadingBuffer = 0.5f;
    public LayerMask DetectionMask;

    [Header("Edge")]
    public bool EdgeDetectionEnabled = true;
    public float EdgeDetectionRayDistance = 0.2f;

    [Header("Wall")]
    public bool WallDetectionEnabled = true;
    public float WallDetection = 0.2f;

    Movement movement;

    NetworkID networkID;

    public void Killed()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        networkID = GetComponent<NetworkID>();
    }

    // Update is called once per frame
    void Update()
    {
        if (networkID.IsMine)
        {
            if (EdgeDetectionEnabled && EdgeDetected())
            {
                moveToRight = !moveToRight;
            }

            if (WallDetectionEnabled && WallDetected())
            {
                moveToRight = !moveToRight;
            }

            if (moveToRight)
            {
                movement.Move(1);
            }
            else
            {
                movement.Move(-1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Debug.Log(other.name);
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.Killed();
        }
    }

    Vector3 DetectionPosition()
    {
        Vector3 detectionPosition = transform.position;

        if (movement.FaceRight)
        {
            detectionPosition += Vector3.right * leadingBuffer;
        }
        else
        {
            detectionPosition -= Vector3.right * leadingBuffer;
        }

        return detectionPosition;
    }

    bool EdgeDetected()
    {
        Debug.DrawRay(DetectionPosition(), Vector2.down * WallDetection, Color.green);
        RaycastHit2D hit2D = Physics2D.Raycast(DetectionPosition(), Vector2.down, EdgeDetectionRayDistance, DetectionMask);

        return hit2D.collider == null;
    }

    bool WallDetected()
    {
        Vector2 forwardDetection;

        if (movement.FaceRight)
        {
            forwardDetection = Vector2.right;
        }
        else
        {
            forwardDetection = Vector2.left;
        }

        Debug.DrawRay(transform.position, forwardDetection * WallDetection, Color.green);
        RaycastHit2D hit2D = Physics2D.Raycast(DetectionPosition(), forwardDetection, WallDetection, DetectionMask);

        bool hitWall = hit2D.collider != null;

        return hitWall;
    }
}
