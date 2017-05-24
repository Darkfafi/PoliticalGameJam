using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overall protester behaviour controller.
/// </summary>
public class TensionManager : MonoBehaviour
{
    public event TensionStateDelegate TensionStateChangedEvent;

    public float TensionRate { get; private set; }
    public bool IsRunning { get; private set; }

    private float _tensionMeter = 0;
    private float _lastTensionMeterValue = 0;

    [SerializeField]
    private GameObject _protestCrowdsRoot;

    [SerializeField]
    private GameObject _protestorSpawnRoot;

    [Header("Gather spot")]

    [SerializeField]
    private RectTransform _protestorsGatherArea;

    [SerializeField]
    private Vector2 _gatherAreaSize = new Vector2(5, 2);

    private StateExpressionBehaviour[] _protestCrowds;

    private int _currentWishedGatherAmount = 5;

    private List<Protester> _currentlySpawnedProtesters = new List<Protester>();

    public void ResumeTensionProgression()
    {
        if (IsRunning) { return; }
        IsRunning = true;
    }

    public void PauseTensionProgression()
    {
        if (!IsRunning) { return; }
        IsRunning = false;
    }
    
    public void Reset(bool runOnReset)
    {
        _tensionMeter = 0;
        _lastTensionMeterValue = 0;
        SetTensionRate(0);
        IsRunning = runOnReset;
    }

    public void SetTensionRate(float value)
    {
        TensionRate = Mathf.Clamp(value, 0, float.MaxValue);
    }

    public TensionState GetTensionState()
    {
        return GetTensionState(_tensionMeter);
    }

    public TensionState GetTensionState(float value)
    {
        if (value < 20)
        {
            return TensionState.Idle;
        }
        else if (value < 60)
        {
            return TensionState.Pushy;
        }
        else if (value < 80)
        {
            return TensionState.Aggression;
        }
        else
        {
            return TensionState.Outbreak;
        }
    }

    protected void Awake()
    {
        SetTensionRate(TensionRate);
        ResumeTensionProgression();
    }

    protected void Update()
    {
        if (!IsRunning) { return; }
        _lastTensionMeterValue = _tensionMeter;
        _tensionMeter = Mathf.Clamp(_tensionMeter + Time.deltaTime * TensionRate, 0, 100);

        if(GetTensionState(_lastTensionMeterValue) != GetTensionState(_tensionMeter))
        {
            for(int i = 0; i < _protestCrowds.Length; i++)
            {
                _protestCrowds[i].SetTensionState(GetTensionState(_tensionMeter));
            }

            if(TensionStateChangedEvent != null)
                TensionStateChangedEvent(GetTensionState(_tensionMeter));
        }

        _protestCrowds = _protestCrowdsRoot.GetComponentsInChildren<StateExpressionBehaviour>();


        if (GetTensionState(_tensionMeter) != TensionState.Idle)
        {
            if ((GetTensionState(_tensionMeter) == TensionState.Pushy && _currentlySpawnedProtesters.Count < 3) || (GetTensionState(_tensionMeter) == TensionState.Aggression && _currentlySpawnedProtesters.Count < 5) || (GetTensionState(_tensionMeter) == TensionState.Outbreak && _currentlySpawnedProtesters.Count < 8))
            {
                SpawnProtestor();
            }
        }

    }

    private void SpawnProtestor()
    {
        Protester p = GameObject.Instantiate<Protester>(Resources.Load<Protester>("Protester"));
        p.transform.SetParent(_protestorSpawnRoot.transform, false);
        _currentlySpawnedProtesters.Add(p);
        Vector2 pos = new Vector2(_protestorsGatherArea.anchoredPosition.x - _gatherAreaSize.x * 50, _protestorsGatherArea.anchoredPosition.y - _gatherAreaSize.y * 50);
        pos.x += _gatherAreaSize.x * (100 * UnityEngine.Random.value);
        pos.y += _gatherAreaSize.y * (100 * UnityEngine.Random.value);
        p.WalkTo(pos, 15);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_protestorsGatherArea.transform.position, _gatherAreaSize);
    }
}
