using UnityEngine;
using System.Collections;
using System;

public class LockedDoor : MonoBehaviour
{
    public static event Action onLockBroken;

    [SerializeField]
    private Animator _doorAnimator;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Hammer")
        {
            _doorAnimator.SetTrigger("Open");

            if (onLockBroken != null)
                onLockBroken();

            gameObject.SetActive(false);
        }
    }
}