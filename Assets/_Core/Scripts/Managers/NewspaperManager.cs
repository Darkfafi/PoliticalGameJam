using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewspaperManager : MonoBehaviour
{
    public static NewspaperManager instance;
    public enum newspaperStates { Neutral, Happy, Unhappy }
    [SerializeField]
    private Image _newspaperImage;
    [SerializeField]
    private Sprite[] _newspaperSprites;
    [SerializeField]
    private Text _newspaperHeadline;
    private bool _swirling;

    private void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void NewspaperSwirl ()
    {
        _newspaperImage.transform.DOScale(12, 2);
        _swirling = true;
    }

    private void NewspaperScaleOut ()
    {
        _newspaperImage.transform.DOScale(0, 0.5f);
    }

    public void PublishNewspaper (string headline, newspaperStates newspaperState)
    {
        _newspaperImage.sprite = _newspaperSprites[(int)newspaperState];
        _newspaperHeadline.text = headline;
        NewspaperSwirl();
    } 

    private void Update()
    {
        if (_newspaperImage.transform.localScale != new Vector3(12, 12, 12) && _swirling == true)
        {
            _newspaperImage.transform.Rotate(0, 0, 500 * Time.deltaTime);
        }
        else if (_swirling == true)
        {
            _newspaperImage.transform.DORotate(new Vector3(0 ,0 ,0), 0.5f);
            Invoke("NewspaperScaleOut", 5f);
            _swirling = false;
        }
    }
}
