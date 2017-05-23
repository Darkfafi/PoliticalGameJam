using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StaticPropController : MonoBehaviour
{
    [SerializeField]
    private Image _propImage;

    public void Awake ()
    {
        _propImage.enabled = false;
    }

    public void MakePropAppear ()
    {
        _propImage.enabled = true;
        _propImage.transform.DOShakeScale(Random.Range(1, 2), 1, 10, 5, true);
    }
}
