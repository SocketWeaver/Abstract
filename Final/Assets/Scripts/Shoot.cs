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
        GameObject bulletGameObject = Instantiate(Bullet, FirePosition.position, FirePosition.rotation);
        Rigidbody2D rb2D = bulletGameObject.GetComponent<Rigidbody2D>();
        Bullet bullet = bulletGameObject.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Owner = gameObject;
        }

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
