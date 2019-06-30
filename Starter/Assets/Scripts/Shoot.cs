using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject Bullet;
    public Transform FirePosition;
    public float BulletSpeed = 10.0f;

    public void FireBullet(bool right)
    {
        GameObject bullet = Instantiate(Bullet, FirePosition.position, FirePosition.rotation);
        Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();

        if (right)
        {
            rb2D.velocity = transform.right * BulletSpeed;
        }
        else
        {
            rb2D.velocity = -(transform.right * BulletSpeed);
        }
    }
}
