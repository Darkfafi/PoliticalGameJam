using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionManager : MonoBehaviour
{
    public event TensionStateDelegate TensionStateChangedEvent;

    public float TensionRate { get; private set; }
    public bool IsRunning { get; private set; }

    private float _tensionMeter = 0;
    private float _lastTensionMeterValue = 0;

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
        TensionRate = Mathf.Clamp(value, 1, float.MaxValue);
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
    }

    protected void Update()
    {
        if (!IsRunning) { return; }
        _lastTensionMeterValue = _tensionMeter;
        _tensionMeter = Mathf.Clamp(_tensionMeter + Time.deltaTime * TensionRate, 0, 100);

        if(GetTensionState(_lastTensionMeterValue) != GetTensionState(_tensionMeter))
        {
            if(TensionStateChangedEvent != null)
                TensionStateChangedEvent(GetTensionState(_tensionMeter));
        }
    }
}
