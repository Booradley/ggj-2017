using UnityEngine;
using System.Collections;
using System;
using Valve.VR.InteractionSystem;

public class HandVelocity : MonoBehaviour
{
    public static event Action onWaveGestureComplete;

    private VelocityEstimator _estimator;
    private Vector3 _lastDirection;
    private float _sampleRate = 0.1f;
    private int _samplesPerWave = 5;
    private int _maxSamplesPerWave = 20;
    private int _wavesPerGesture = 4;

    private int _currentSuccessfulSamples = 0;
    private int _currentSuccessfulWaves = 0;

    private void Start()
    {
        _estimator = GetComponent<VelocityEstimator>();
    }

    public void SampleWaves()
    {
        StartCoroutine(SampleWavesSequence());
    }

    private IEnumerator SampleWavesSequence()
    {
        int sample = 0;
        _currentSuccessfulSamples = 0;
        _currentSuccessfulWaves = 0;

        while (true)
        {
            Vector3 velocity = _estimator.GetVelocityEstimate();
            float distance = Vector3.Distance(_lastDirection, velocity.normalized);
            
            if (distance < 1.5f && velocity.magnitude > 0.5f)
            {
                _currentSuccessfulSamples++;
            }

            if (_currentSuccessfulSamples == _samplesPerWave)
            {
                _currentSuccessfulWaves++;
                _currentSuccessfulSamples = 0;
                sample = 0;

                Debug.LogFormat("WAVE {0}", _currentSuccessfulWaves);

                if (_currentSuccessfulWaves >= _wavesPerGesture)
                {
                    Debug.Log("WAVED");
                    if (onWaveGestureComplete != null)
                        onWaveGestureComplete();

                    _currentSuccessfulWaves = 0;
                }
            }
            else if (sample > _maxSamplesPerWave)
            {
                Debug.Log("FAILED WAVE");
                _currentSuccessfulWaves = 0;
                _currentSuccessfulSamples = 0;
                sample = 0;
            }

            _lastDirection = velocity.normalized;
            sample++;

            yield return new WaitForSeconds(_sampleRate);
        }
    }
}