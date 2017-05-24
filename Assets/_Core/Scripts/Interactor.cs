using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public bool IsInteracting { get; private set; }

    [SerializeField]
    private Animator _animator;

    private float _speed;
    private Vector2? _endPosition = null;

    public void SetInteractionState(bool value)
    {
        IsInteracting = value;
    }

    public void WalkTo(Vector2 endPosition, float speed)
    {
        _endPosition = endPosition;
        _speed = speed;
        _animator.SetFloat("WalkSpeed", _speed);
    }

    public static T GetFreeInteractor<T>(T[] interactors) where T : Interactor
    {
        List<T> free = new List<T>();

        for(int i = interactors.Length - 1; i >= 0; i--)
        {
            if(!interactors[i].IsInteracting)
            {
                free.Add(interactors[i]);
            }
        }

        if (free.Count > 0)
            return free[UnityEngine.Random.Range(0, free.Count)];
        else
            return null;
    }

    protected void Update()
    {
        RectTransform ownPos = (RectTransform)transform;
        if(_endPosition.HasValue)
        {
            Vector2 diff = _endPosition.Value - new Vector2(ownPos.anchoredPosition.x, ownPos.anchoredPosition.y);
            Vector2 dir = diff.normalized;

            dir *= _speed * Time.deltaTime * 20;

            if (diff.magnitude < dir.magnitude)
            {
                ownPos.anchoredPosition = _endPosition.Value;
            }
            else
            {
                Vector2 t = ownPos.anchoredPosition;
                t += dir;
                ((RectTransform)transform).anchoredPosition = t;
            }

            if((_endPosition.Value - new Vector2(ownPos.anchoredPosition.x, ownPos.anchoredPosition.y)).magnitude < 2 * Time.deltaTime)
            {
                ownPos.anchoredPosition = _endPosition.Value;
                _endPosition = null;
            }
        }
        else
        {
            _animator.SetFloat("WalkSpeed", 0);
        }
    }
}
