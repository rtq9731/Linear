using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlowerController : MonoSingleton<FlowerController>
{
    [SerializeField]
    private Image flowerImage;

    [SerializeField]
    private Sprite[] flowerSprites;

    public void SetFlowerLevel(int level)
    {
        flowerImage.sprite = flowerSprites[level - 1];
    }
}
