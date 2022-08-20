using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowController : MonoSingleton<ArrowController>
{
    [SerializeField]
    private Sprite arrowSprite = null;

    [SerializeField]
    private Sprite doubleArrowSprite = null;

    [SerializeField]
    private Sprite place = null;

    [SerializeField]
    private Image[] arrowImages = null;

    public void SetArrow(StateType type, float addValue)
    {
        if (addValue == 0)
        {
            arrowImages[(int)type].sprite = place;
            return;
        }
        arrowImages[(int)type].sprite = addValue > 20 ? doubleArrowSprite : arrowSprite;
        arrowImages[(int)type].transform.rotation = Quaternion.Euler(0, 0, addValue < 0 ? 0 : 180);
    }
}
