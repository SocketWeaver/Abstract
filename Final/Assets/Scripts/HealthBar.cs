using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public void UpdateHealth(float percentage)
    {
        Vector3 localScale = transform.localScale;
        localScale.x = percentage;
        transform.localScale = localScale;
    }
}
