﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWNetwork;

public class Player : MonoBehaviour
{
    public float FiringSpeed = 1.0f;

    Movement movement;
    Shoot shoot;
    Animator anim;

    NetworkID networkID;

    public void Killed()
    {
        Debug.Log("Game Over");
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        shoot = GetComponent<Shoot>();
        networkID = GetComponent<NetworkID>();

        if (networkID.IsMine)
        {
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            cameraFollow.Target = transform;
        }
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

            anim.SetBool("ground", movement.Ground);
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
    }
}