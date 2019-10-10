using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float DestoryDelay = 1;
    public int Damage = 1;

    public GameObject Owner;

    private void Start()
    {
        Destroy(gameObject, DestoryDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other == Owner)
        {
            return;
        }

        IHealth health = (IHealth)other.GetComponent(typeof(IHealth));

        if (health != null)
        {
            health.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}
