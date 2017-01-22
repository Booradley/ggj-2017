using UnityEngine;
using System.Collections;
using System;

public class DarkRoomTrigger : MonoBehaviour
{
    public static event Action onDarkRoomTriggerEntered;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (onDarkRoomTriggerEntered != null)
                onDarkRoomTriggerEntered();
        }
    }
}