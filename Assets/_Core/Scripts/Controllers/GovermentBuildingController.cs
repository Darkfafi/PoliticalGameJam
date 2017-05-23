using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GovermentBuildingController : MonoBehaviour
{
    public enum govermentBuildingStates { Neutral, Peaceful, Defended, Damaged }
    private govermentBuildingStates _govermentBuildingState;
    [SerializeField]
    private Image _govermentBuildingImage;
    [SerializeField]
    private Sprite[] _govermentBuildingSprites;

    private void SetGovermentBuildingSprite ()
    {
        Camera.main.DOShakePosition(Random.Range(2, 4), 15, 15, 15, true);
        _govermentBuildingImage.sprite = _govermentBuildingSprites[(int)_govermentBuildingState];
    }

    public govermentBuildingStates GovermentBuildingState
    {
        set
        {
            _govermentBuildingState = value;
            SetGovermentBuildingSprite();
        }
    }
}
