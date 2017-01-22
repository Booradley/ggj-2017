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

    [SerializeField]
    private Transform _doorForce;

    [SerializeField]
    private AudioClip _safeOpenSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

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
        _doorRigidBody.AddExplosionForce(50f, _doorForce.position, 0.5f);

        // Play sound
        _audioSource.PlayOneShot(_safeOpenSound);

        if (onSafeOpened != null)
            onSafeOpened();
    }
}