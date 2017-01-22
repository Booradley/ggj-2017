using UnityEngine;
using System.Collections;
using System;

public class TreeTrigger : MonoBehaviour
{
    public static event Action onTreeTriggerEntered;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (onTreeTriggerEntered != null)
                onTreeTriggerEntered();

            gameObject.SetActive(false);
        }
    }
}