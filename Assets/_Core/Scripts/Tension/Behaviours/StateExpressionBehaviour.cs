using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class StateExpressionBehaviour : MonoBehaviour
{
    private TensionState _currentState;

    [SerializeField]
    private TensionState _preTensionState = TensionState.Idle;

    private Animator _animator;

    public void SetTensionState(TensionState tensionState)
    {
        _preTensionState = tensionState;
        _currentState = tensionState;

        SetStateAnimations(tensionState);
    }

    protected void Awake()
    {
        SetTensionState(TensionState.Idle);
        _animator = this.GetComponent<Animator>();
        _animator.speed = _animator.speed - 0.1f + (0.3f * UnityEngine.Random.value);
    }

    protected void Update()
    {
        if(_preTensionState != _currentState)
        {
            SetTensionState(_preTensionState);
        }
    }

    private void SetStateAnimations(TensionState tensionState)
    {
        this.transform.DOKill();
        this.transform.localScale = new Vector3(1, 1, 1);

        switch (tensionState)
        {
            case TensionState.Idle:
                this.transform.DOScale(1.05f, 0.85f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
            case TensionState.Pushy:

                this.transform.DOScale(1.05f, 0.85f).SetEase(Ease.OutElastic).OnComplete(()=> {

                    this.transform.DOScale(1, 0.4f).OnComplete(()=> { SetStateAnimations(TensionState.Pushy); });
                });
                break;
            case TensionState.Aggression:
                this.transform.DOScale(1.08f, 0.35f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBack);
                break;

            case TensionState.Outbreak:
                this.transform.DOScale(1.10f, 0.185f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InBack);
                break;
        }
    }
}
