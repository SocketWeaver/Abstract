using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class Player : MonoBehaviour
{
    public float FiringSpeed = 1.0f;
    public int Health = 0;

    Movement movement;
    Shoot shoot;
    Animator anim;

    NetworkID networkID;

    const string FIRE = "fire";
    const int MaxHealth = 5;
    RemoteEventAgent remoteEventAgent;
    HealthBar healthBar;

    public void TakeDamage(int damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, MaxHealth);
        if (healthBar != null)
        {
            float percentage = (float)Health / (float)MaxHealth;
            healthBar.UpdateHealth(percentage);
        }
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        shoot = GetComponent<Shoot>();
        networkID = GetComponent<NetworkID>();
        remoteEventAgent = GetComponent<RemoteEventAgent>();
        healthBar = GetComponentInChildren<HealthBar>();

        if (networkID.IsMine)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.Target = transform;
        }

        Health = MaxHealth;
    }

    void Update()
    {
        if (networkID.IsMine)
        {
            // movement
            if (movement.Ground && Input.GetKeyDown(KeyCode.Space))
            {
                movement.Jump();
            }

            float move = Input.GetAxis("Horizontal");

            movement.Move(move);

            anim.SetBool("air", !movement.Ground);
            anim.SetFloat("speed", Mathf.Abs(move));

            // shoot
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!IsInvoking("Fire"))
                {
                    InvokeRepeating("Fire", 0f, FiringSpeed);
                }
            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                CancelInvoke("Fire");
            }
        }
    }

    void Fire()
    {
        shoot.FireBullet(movement.FaceRight);

        SWNetworkMessage message = new SWNetworkMessage();
        message.Push(movement.FaceRight);
        remoteEventAgent.Invoke(FIRE, message);
    }

    public void RemoteFired(SWNetworkMessage message)
    {
        bool faceRight = message.PopBool();
        shoot.FireBullet(faceRight);
    }
}
