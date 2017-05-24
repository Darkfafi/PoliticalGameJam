using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField]
    private GameObject _policeSpawnRoot;

    [SerializeField]
    private GameObject _policeCrowdsRoot;

    [SerializeField]
    private StateExpressionBehaviour[] _policeCrowds;

    [SerializeField]
    private List<CanvasGroup> _crowdsCanvasGroups = new List<CanvasGroup>();

    [SerializeField]
    private RectTransform _protestorsGatherArea;

    [SerializeField]
    private Vector2 _gatherAreaSize = new Vector2(3, 1.2f);

    private List<Police> _policeSpawned = new List<Police>();

    private bool _setPoliceOnPoint = false;

    private float _timeSinceInteraction = 0;
    private int _indexShow = 0;

    private List<List<Police>> _police = new List<List<Police>>();

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
        _interactionSystem.InteractionEndedEvent += OnInteractionEndedEvent;

        _policeCrowds = this._policeCrowdsRoot.GetComponentsInChildren<StateExpressionBehaviour>();

        for (int i = 0; i < _policeCrowds.Length; i++)
        {
            CanvasGroup cg = _policeCrowds[i].GetComponentInParent<CanvasGroup>();
            if(!_crowdsCanvasGroups.Contains(cg))
            {
                _crowdsCanvasGroups.Add(cg);
                cg.alpha = 0;
            }
        }
    }

    protected void OnDestroy()
    {
        _tensionManager.TensionStateChangedEvent -= OnTensionStateChangedEvent;
        _interactionSystem.InteractionEndedEvent -= OnInteractionEndedEvent;
    }

    protected void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_setPoliceOnPoint)
            {
                SetTensionState(GetNextNaturalTensionState(TensionState));
            }
            else
            {
                _setPoliceOnPoint = true;
            }

            if (_indexShow <= _crowdsCanvasGroups.Count - 1)
            { 
                _crowdsCanvasGroups[this._indexShow].alpha = 1;
                _crowdsCanvasGroups[this._indexShow].transform.localScale = new Vector3(0, 0, 0);
                _crowdsCanvasGroups[this._indexShow].transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
                _indexShow++;
            }
        }

        _timeSinceInteraction += Time.deltaTime;


        if (TensionState != TensionState.Idle)
        {
            if ((TensionState == TensionState.Pushy && _policeSpawned.Count < 3) || (TensionState == TensionState.Aggression && _policeSpawned.Count < 5) || (TensionState == TensionState.Outbreak && _policeSpawned.Count < 8))
            {
                SpawnPolice();
            }
        }

        if (_timeSinceInteraction > 4 + UnityEngine.Random.value * 4)
        {
            _timeSinceInteraction = 0;

            Vector2 interactionLocation = ((RectTransform)transform).anchoredPosition;

            interactionLocation.x -= 30;
            interactionLocation.x += 60 * UnityEngine.Random.value;
            interactionLocation.y -= 60;
            interactionLocation.y += 120 * UnityEngine.Random.value;


            switch (TensionState)
            {
                case TensionState.Aggression:
                    _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Fighting, interactionLocation);
                    break;

                case TensionState.Idle:
                    // DO NOTHING
                    break;
                case TensionState.Pushy:
                    _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Pushing, interactionLocation);
                    break;
                case TensionState.Outbreak:
                    _interactionSystem.TryInteractionMatch(InteractionSystem.InteractionType.Murder, interactionLocation);
                    break;
            }
        }
    }

    private void OnInteractionEndedEvent(InteractionSystem.InteractionType interactionType)
    {
        if(interactionType == InteractionSystem.InteractionType.Murder)
        {
            _tensionManager.SetTensionRate(1001337);
        }
    }

    private void SetTensionState(TensionState tensionState)
    {
        TensionState = tensionState;
        float newRate = 1;
        switch (tensionState)
        {
            case TensionState.Aggression:
                newRate = _tensionManager.TensionRate + 3;
                break;

            case TensionState.Idle:
                newRate = 0;
                break;
            case TensionState.Pushy:
                newRate = _tensionManager.TensionRate + 1;
                break;
            case TensionState.Outbreak:
                newRate = _tensionManager.TensionRate + 4;
                break;
        }

        for (int i = 0; i < _policeCrowds.Length; i++)
        {
            _policeCrowds[i].SetTensionState(TensionState);
        }

        _tensionManager.SetTensionRate(newRate);
    }

    private void OnTensionStateChangedEvent(TensionState tensionState)
    {
        if(tensionState == TensionState.Outbreak)
        {
            SetTensionState(TensionState.Outbreak);
        }
    }

    private void SpawnPolice()
    {
        Police p = GameObject.Instantiate<Police>(Resources.Load<Police>("Police"));
        p.transform.SetParent(_policeSpawnRoot.transform, false);
        _policeSpawned.Add(p);
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
