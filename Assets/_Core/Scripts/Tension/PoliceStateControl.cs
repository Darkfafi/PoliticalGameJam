using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overall Police controll
/// </summary>
public class PoliceStateControl : MonoBehaviour
{
    public TensionState TensionState { get; private set; }

    [SerializeField]
    private TensionManager _tensionManager;

    [SerializeField]
    private InteractionSystem _interactionSystem;

    private bool _setPoliceOnPoint = false;

    private float _timeSinceInteraction = 0;

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
        _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Pushing, ((RectTransform)transform).anchoredPosition);
    }

    protected void OnDestroy()
    {
        _tensionManager.TensionStateChangedEvent -= OnTensionStateChangedEvent;
    }

    protected void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_setPoliceOnPoint)
                SetTensionState(GetNextNaturalTensionState(TensionState)); 
            else
            {
                _setPoliceOnPoint = true;
                // TODO: Spawn Police troops.
            }
        }

        _timeSinceInteraction += Time.deltaTime;

        if (_timeSinceInteraction > 4 + UnityEngine.Random.value * 4)
        {
            _timeSinceInteraction = 0;

            Vector2 interactionLocation = ((RectTransform)transform).anchoredPosition;

            switch (_tensionManager.GetTensionState())
            {
                case TensionState.Aggression:
                    //TODO: x percentage chance on murder which grows & after murder go to outbreak state for both groups. else fighting and pushing
                    break;

                case TensionState.Idle:
                    // DO NOTHING
                    break;
                case TensionState.Pushy:
                    _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Pushing, interactionLocation);
                    break;
                case TensionState.Outbreak:
                    // TODO: FULL MURDER AND FIGHTING
                    break;
            }
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
