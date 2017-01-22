using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// This class handles some basic properties of physical objects
/// </summary>
public class PhysicalObject : MonoBehaviour
{
    private const float SECONDS_BETWEEN_PLAYBACK = 0.1f;

    public enum MaterialType { Wet, Soft, Hard }

    public enum ImpactForce { Soft, Medium, Hard }

    [SerializeField]
    protected MaterialType _materialType;
    public MaterialType materialType { get { return _materialType; } }

    [SerializeField]
    protected AudioSource _audioSource;

    [SerializeField]
    protected Rigidbody _rigidBody;
    public Rigidbody rigidBody { get { return _rigidBody; } }

    [SerializeField]
    protected AudioClip[] _loudHitSounds;
    public AudioClip[] loudHitSounds { get { return _loudHitSounds; } }

    [SerializeField]
    protected AudioClip[] _mediumHitSounds;
    public AudioClip[] mediumHitSounds { get { return _mediumHitSounds; } }

    [SerializeField]
    protected AudioClip[] _softHitSounds;
    public AudioClip[] softHitSounds { get { return _softHitSounds; } }

    [SerializeField]
    protected float _loudHitSoundVelocityThreshold;
    public float loudHitSoundVelocityThreshold { get { return _loudHitSoundVelocityThreshold; } }

    [SerializeField]
    protected float _mediumHitSoundVelocityThreshold;
    public float mediumHitSoundVelocityThreshold { get { return _mediumHitSoundVelocityThreshold; } }

    [SerializeField]
    protected float _softHitSoundVelocityThreshold;
    public float softHitSoundVelocityThreshold { get { return _softHitSoundVelocityThreshold; } }

    private float _lastPlayTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.realtimeSinceStartup - _lastPlayTime < SECONDS_BETWEEN_PLAYBACK)
            return;

        PhysicalObject otherObject = collision.transform.GetComponentInParent<PhysicalObject>();
        if (_rigidBody != null && _rigidBody.velocity.magnitude >= _softHitSoundVelocityThreshold)
        {
            if (otherObject != null)
            {
                PhysicalObjectImpactHandler.RegisterImpact(this, otherObject);
            }
            else
            {
                PhysicalObjectImpactHandler.PlayImpactSound(this, GetImpactForce(null), GetImpactVolume(null));
            }
        }
    }

    public ImpactForce GetImpactForce(PhysicalObject otherObject)
    {
        if (_rigidBody != null)
        {
            if (_rigidBody.velocity.magnitude >= _loudHitSoundVelocityThreshold)
            {
                return ImpactForce.Hard;
            }
            else if (_rigidBody.velocity.magnitude >= _mediumHitSoundVelocityThreshold)
            {
                return ImpactForce.Medium;
            }
            else
            {
                return ImpactForce.Soft;
            }
        }
        else if (otherObject != null && otherObject.rigidBody != null)
        {
            if (otherObject.rigidBody.velocity.magnitude >= _loudHitSoundVelocityThreshold)
            {
                return ImpactForce.Hard;
            }
            else if (otherObject.rigidBody.velocity.magnitude >= _mediumHitSoundVelocityThreshold)
            {
                return ImpactForce.Medium;
            }
            else
            {
                return ImpactForce.Soft;
            }
        }

        return ImpactForce.Medium;
    }

    public float GetImpactVolume(PhysicalObject otherObject)
    {
        if (_rigidBody != null)
        {
            if (_rigidBody.velocity.magnitude >= _loudHitSoundVelocityThreshold)
            {
                return 1f;
            }
            else if (_rigidBody.velocity.magnitude >= _mediumHitSoundVelocityThreshold)
            {
                return Mathf.Clamp((_rigidBody.velocity.magnitude - _mediumHitSoundVelocityThreshold) / (_loudHitSoundVelocityThreshold - _mediumHitSoundVelocityThreshold), 0.5f, 1f);
            }
            else
            {
                return (_rigidBody.velocity.magnitude - _softHitSoundVelocityThreshold) / (_mediumHitSoundVelocityThreshold - _softHitSoundVelocityThreshold);
            }
        }
        else if (otherObject != null && otherObject.rigidBody != null)
        {
            if (otherObject.rigidBody.velocity.magnitude >= _loudHitSoundVelocityThreshold)
            {
                return 1f;
            }
            else if (otherObject.rigidBody.velocity.magnitude >= _mediumHitSoundVelocityThreshold)
            {
                return Mathf.Clamp((otherObject.rigidBody.velocity.magnitude - _mediumHitSoundVelocityThreshold) / (_loudHitSoundVelocityThreshold - _mediumHitSoundVelocityThreshold), 0.5f, 1f);
            }
            else
            {
                return (otherObject.rigidBody.velocity.magnitude - _softHitSoundVelocityThreshold) / (_mediumHitSoundVelocityThreshold - _softHitSoundVelocityThreshold);
            }
        }

        return 1f;
    }

    public void PlayHitSound(ImpactForce impactForce, float volume)
    {
        if (Time.realtimeSinceStartup - _lastPlayTime < SECONDS_BETWEEN_PLAYBACK)
            return;
        
        AudioClip[] clips = _softHitSounds;
        if (impactForce == ImpactForce.Medium)
        {
            clips = _mediumHitSounds;
        }
        else if (impactForce == ImpactForce.Hard)
        {
            clips = _loudHitSounds;
        }

        if (clips.Length > 0)
        {
            _audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)], volume);
        }

        _lastPlayTime = Time.realtimeSinceStartup;
    }
}

public static class PhysicalObjectImpactHandler
{
    private struct PhysicalObjectImpact
    {
        public PhysicalObject physicalObject1;
        public PhysicalObject.ImpactForce physicalObject1Force;
        public float physicalObject1Volume;
        public PhysicalObject physicalObject2;
        public PhysicalObject.ImpactForce physicalObject2Force;
        public float physicalObject2Volume;

        public override bool Equals(object obj)
        {
            return obj is PhysicalObjectImpact && this == (PhysicalObjectImpact)obj;
        }

        public override int GetHashCode()
        {
            return physicalObject1.GetHashCode() ^ physicalObject2.GetHashCode();
        }

        public static bool operator ==(PhysicalObjectImpact x, PhysicalObjectImpact y)
        {
            return (x.physicalObject1 == y.physicalObject1 && x.physicalObject2 == y.physicalObject2) || (x.physicalObject1 == y.physicalObject2 && x.physicalObject2 == y.physicalObject1);
        }

        public static bool operator !=(PhysicalObjectImpact x, PhysicalObjectImpact y)
        {
            return !(x == y);
        }
    }

    private static List<PhysicalObject.MaterialType> _materialImpactSoundPrecedence = new List<PhysicalObject.MaterialType>()
    {
        PhysicalObject.MaterialType.Wet,
        PhysicalObject.MaterialType.Soft,
        PhysicalObject.MaterialType.Hard
    };

    private static List<PhysicalObjectImpact> _registeredImpacts;

    /// <summary>
    /// Registered impacts between 2 physical objects get handled each frame
    /// </summary>
    public static void RegisterImpact(PhysicalObject physicalObject1, PhysicalObject physicalObject2)
    {
        PhysicalObjectImpact impact = new PhysicalObjectImpact();
        impact.physicalObject1 = physicalObject1;
        impact.physicalObject1Force = physicalObject1.GetImpactForce(physicalObject2);
        impact.physicalObject1Volume = physicalObject1.GetImpactVolume(physicalObject2);
        impact.physicalObject2 = physicalObject2;
        impact.physicalObject2Force = physicalObject2.GetImpactForce(physicalObject1);
        impact.physicalObject2Volume = physicalObject2.GetImpactVolume(physicalObject1);

        if (_registeredImpacts == null)
        {
            _registeredImpacts = new List<PhysicalObjectImpact>();
        }

        if (!_registeredImpacts.Contains(impact))
        {
            _registeredImpacts.Add(impact);
        }
    }

    public static void HandleRegisteredImpacts()
    {
        if (_registeredImpacts == null)
            return;

        for (int i = 0; i < _registeredImpacts.Count; i++)
        {
            int object1Index = _materialImpactSoundPrecedence.IndexOf(_registeredImpacts[i].physicalObject1.materialType);
            int object2Index = _materialImpactSoundPrecedence.IndexOf(_registeredImpacts[i].physicalObject2.materialType);

            if (object1Index > object2Index)
            {
                _registeredImpacts[i].physicalObject1.PlayHitSound(_registeredImpacts[i].physicalObject1Force, _registeredImpacts[i].physicalObject1Volume);
            }
            else if (object2Index > object1Index)
            {
                _registeredImpacts[i].physicalObject2.PlayHitSound(_registeredImpacts[i].physicalObject2Force, _registeredImpacts[i].physicalObject2Volume);
            }
            else
            {
                float rand = UnityEngine.Random.Range(0f, 1f);
                if (rand < 0.5f)
                {
                    _registeredImpacts[i].physicalObject1.PlayHitSound(_registeredImpacts[i].physicalObject1Force, _registeredImpacts[i].physicalObject1Volume);
                }
                else
                {
                    _registeredImpacts[i].physicalObject2.PlayHitSound(_registeredImpacts[i].physicalObject2Force, _registeredImpacts[i].physicalObject2Volume);
                }
            }
        }

        _registeredImpacts.Clear();
    }

    public static void PlayImpactSound(PhysicalObject physicalObject, PhysicalObject.ImpactForce impactForce, float volume)
    {
        physicalObject.PlayHitSound(impactForce, volume);
    }
}