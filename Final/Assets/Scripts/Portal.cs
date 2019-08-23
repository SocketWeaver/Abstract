using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform Destination;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(Destination!= null)
        {
            GameObject other = collision.gameObject;
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.transform.position = Destination.position;
            }
        }
    }
}
