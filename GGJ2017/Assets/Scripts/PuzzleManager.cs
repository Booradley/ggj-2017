using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance;
    public static PuzzleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PuzzleManager>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private List<SegmentData> _segments;

    private SegmentData _currentSegmentData;
    private int _currentSegmentIndex = 0;
    private Coroutine _segmentCoroutine;

    private void Start()
    {
        PlayNextSegment();
    }

    private void PlayNextSegment()
    {
        if (_segmentCoroutine != null)
        {
            StopCoroutine(_segmentCoroutine);
        }

        if (_currentSegmentData != null)
        {
            _currentSegmentIndex++;
        }

        _currentSegmentData = _segments[_currentSegmentIndex];
        _segmentCoroutine = StartCoroutine(RunSegmentSequence());
    }

    private IEnumerator RunSegmentSequence()
    {
        yield return null;
    }
}