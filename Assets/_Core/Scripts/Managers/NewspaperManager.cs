using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewspaperManager : MonoBehaviour
{
    [SerializeField]
    private string[] _headlines;
    [SerializeField]
    private Image _newspaperImage;
    private bool _swirling;

    private void NewspaperSwirl ()
    {
        _newspaperImage.transform.DOScale(12, 2);
        _swirling = true;
    }

    private void Update()
    {
        if (_newspaperImage.transform.localScale != new Vector3(12, 12, 12) && _swirling == true)
        {
            _newspaperImage.transform.Rotate(0, 0, 500 * Time.deltaTime);
        }
        else if (_newspaperImage.transform.localScale == new Vector3(12, 12, 12) && _newspaperImage.transform.rotation != new Quaternion(0, 0, 0, 0) && _swirling == true)
        {
            _newspaperImage.transform.DORotate(new Vector3(0 ,0 ,0), 0.5f);
        }

        if(Input.anyKeyDown)
        {
            NewspaperSwirl();
        }
    }
}
