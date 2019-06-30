using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        Enemy enemy = other.GetComponent<Enemy>();

        if(enemy != null)
        {
            enemy.Killed();
        }

        Destroy(gameObject);
    }
}
