using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceStateControl : MonoBehaviour
{
    public TensionState TensionState { get; private set; }

    [SerializeField]
    private TensionManager _tensionManager;

    [SerializeField]
    private InteractionSystem _interactionSystem;

    public static TensionState GetNextNaturalTensionState(TensionState tensionState)
    {
        switch(tensionState)
        {
            case TensionState.Idle:
                return TensionState.Pushy;
            case TensionState.Pushy:
                return TensionState.Aggression;
            case TensionState.Aggression:
                return TensionState.Outbreak;
            case TensionState.Outbreak:
                return TensionState.Outbreak;
        }

        Debug.LogWarning("No Tension return state set for the following state: " + tensionState.ToString());
        return TensionState.Idle;
    }

    protected void Awake()
    {
        _tensionManager.TensionStateChangedEvent += OnTensionStateChangedEvent;
        _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Pusing, ((RectTransform)transform).anchoredPosition);
    }

    protected void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            SetTensionState(GetNextNaturalTensionState(TensionState)); 
        }
	}

    private void SetTensionState(TensionState tensionState)
    {
        TensionState = tensionState;
    }

    private void OnTensionStateChangedEvent(TensionState tensionState)
    {
        if(tensionState == TensionState.Outbreak)
        {
            SetTensionState(TensionState.Outbreak);
        }
    }
}
