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

        if (_currentSegmentIndex < _segments.Count)
        {
            Debug.LogFormat("Playing segment {0}", _currentSegmentIndex);
            _currentSegmentData = _segments[_currentSegmentIndex];
            _segmentCoroutine = StartCoroutine(RunSegmentSequence());
        }
        else
        {
            // All segments complete
            DialogManager.Instance.Reset();

            Debug.Log("All segments complete");
        }
    }

    private IEnumerator RunSegmentSequence()
    {
        _currentSegmentData.Setup();

        DialogManager.Instance.AddDialogMulti(_currentSegmentData.initialDialog);
        DialogManager.Instance.AddSecondaryDialogMulti(_currentSegmentData.randomDialog);

        while (!_currentSegmentData.isComplete)
        {
            yield return null;
        }

        _currentSegmentData.Cleanup();
        _segmentCoroutine = null;

        PlayNextSegment();
    }
}