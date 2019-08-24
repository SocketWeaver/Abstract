using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform Destination;

    public void OnPlayerEntered(Player player)
    {
        if (player != null)
        {
            player.transform.position = Destination.position;
        }
    }
}
