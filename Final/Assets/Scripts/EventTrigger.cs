using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [System.Serializable]
    public class EnteredTriggerEvent : UnityEvent<Player>
    {
    }
    public bool OnlyTriggerOnce = false;
    bool alreadyTriggered = false;

    public EnteredTriggerEvent playeredEnteredEvent = new EnteredTriggerEvent();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(alreadyTriggered && OnlyTriggerOnce)
        {
            return;
        }

        GameObject other = collision.gameObject;
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            alreadyTriggered = true;
            playeredEnteredEvent.Invoke(player);
        }
    }
}
