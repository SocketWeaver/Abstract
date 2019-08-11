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
    const string HEALTH = "Health";
    const int MaxHealth = 5;
    RemoteEventAgent remoteEventAgent;
    SyncPropertyAgent syncPropertyAgent;
    HealthBar healthBar;

    public void TakeDamage(int damage)
    {
        int health = syncPropertyAgent.GetPropertyWithName(HEALTH).GetIntValue();
        health = Mathf.Clamp(health - damage, 0, MaxHealth);
        if (NetworkClient.Instance.IsHost)
        {
            syncPropertyAgent.Modify(HEALTH, health);
        }
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        shoot = GetComponent<Shoot>();
        networkID = GetComponent<NetworkID>();
        remoteEventAgent = GetComponent<RemoteEventAgent>();
        syncPropertyAgent = GetComponent<SyncPropertyAgent>();

        healthBar = GetComponentInChildren<HealthBar>();

        if (networkID.IsMine)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.Target = transform;
        }

        Health = MaxHealth;
    }

    public void OnHealthSyncPropertyReady()
    {
        int health = syncPropertyAgent.GetPropertyWithName(HEALTH).GetIntValue();
        int version = syncPropertyAgent.GetPropertyWithName(HEALTH).version;

        if(version == 0)
        {
            syncPropertyAgent.Modify(HEALTH, MaxHealth);
            health = MaxHealth;
        }

        UpdateHealthBar(health);
    }

    public void OnHealthSyncPropertyChanged()
    {
        int health = syncPropertyAgent.GetPropertyWithName(HEALTH).GetIntValue();
        UpdateHealthBar(health);
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

    void UpdateHealthBar(int currentHealth)
    {
        if (healthBar != null)
        {
            float percentage = (float)currentHealth / (float)MaxHealth;
            healthBar.UpdateHealth(percentage);
        }
    }

    public void RemoteFired(SWNetworkMessage message)
    {
        bool faceRight = message.PopBool();
        shoot.FireBullet(faceRight);
    }
}
