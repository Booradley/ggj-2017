using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;
using System;

public class Safe : MonoBehaviour
{
    public static event Action onSafeOpened;

    [SerializeField]
    private Rigidbody _doorRigidBody;

    [SerializeField]
    private HingedObject _doorHinge;

    [SerializeField]
    private Keypad _keypad;

    private void Awake()
    {
        _doorRigidBody.isKinematic = true;
        _doorHinge.Disable();

        _keypad.onCodeCorrect += HandleCodeCorrect;
    }

    private void HandleCodeCorrect()
    {
        _keypad.onCodeCorrect -= HandleCodeCorrect;
        _keypad.Disable();

        _doorRigidBody.isKinematic = false;
        _doorHinge.Enable();

        // Apply force to door

        // Play sound
    }
}