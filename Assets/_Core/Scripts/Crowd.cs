using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crowd : MonoBehaviour
{
    [SerializeField]
    private Image _sign1;

    [SerializeField]
    private Image _sign2;

    [SerializeField]
    private Image _sign3;

    [SerializeField]
    private Sprite[] _normal;

    [SerializeField]
    private Sprite[] _deadly;

    public void setTensionStateSigns(TensionState state)
    {
        float percNormal = 0;

        switch(state)
        {
            case TensionState.Idle:
                percNormal = 100;
                break;

            case TensionState.Pushy:
                percNormal = 60;
                break;

            case TensionState.Aggression:
                percNormal = 50;
                break;

            case TensionState.Outbreak:
                percNormal = 0;
                break;
        }

        if(percNormal > (UnityEngine.Random.value * 100))
        {
            _sign1.sprite = _normal[UnityEngine.Random.Range(0, (_normal.Length))];
        }
        else
        {
            _sign1.sprite = _deadly[UnityEngine.Random.Range(0, (_deadly.Length))];
        }
        if (percNormal > (UnityEngine.Random.value * 100))
        {
            _sign2.sprite = _normal[UnityEngine.Random.Range(0, (_normal.Length))];
        }
        else
        {
            _sign2.sprite = _deadly[UnityEngine.Random.Range(0, (_deadly.Length))];
        }
        if (percNormal > (UnityEngine.Random.value * 100))
        {
            _sign3.sprite = _normal[UnityEngine.Random.Range(0, (_normal.Length))];
        }
        else
        {
            _sign3.sprite = _deadly[UnityEngine.Random.Range(0, (_deadly.Length))];
        }
    }

    protected void Awake()
    {
        setTensionStateSigns(TensionState.Idle);
    }
}
