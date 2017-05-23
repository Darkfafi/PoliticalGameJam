using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GovermentBuildingController : MonoBehaviour
{
    private enum _govermentBuildingStates { Neutral, Peaceful, Defended, Damaged }
    private _govermentBuildingStates _govermentBuildingState;
    [SerializeField]
    private Image _govermentBuildingImage;
    [SerializeField]
    private Sprite[] _govermentBuildingSprites;

    void Awake ()
    {
        _govermentBuildingState = _govermentBuildingStates.Neutral;
    }

    private void SetGovermentBuildingSprite ()
    {
        switch (_govermentBuildingState)
        {
            case _govermentBuildingStates.Neutral:
                break;
            case _govermentBuildingStates.Peaceful:
                break;
            case _govermentBuildingStates.Defended:
                break;
            case _govermentBuildingStates.Damaged:
                break;
        }
        _govermentBuildingImage.sprite = _govermentBuildingSprites[(int)_govermentBuildingState];
    }
}
