using UnityEngine;
using System.Collections;
using System;

public class LockedDoor : MonoBehaviour
{
    public static event Action onLockBroken;

    [SerializeField]
    private Animator _doorAnimator;

    private bool _isOpen = false;

    public void Open()
    {
        if (_isOpen)
            return;

        _doorAnimator.SetTrigger("Open");
        _isOpen = true;

        if (onLockBroken != null)
            onLockBroken();

        gameObject.SetActive(false);
    }

    
}