using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    [SerializeField] Text text;

    public void InitTextEffect(string str, Vector2 pos, Color color)
    {
        text.color = color;
        text.transform.localScale = Vector3.one;
        transform.position = pos;
        gameObject.SetActive(true);
        text.text = str;

        text.DOFade(0, 1f);
        text.transform.DOScale(0f, 1f).SetEase(Ease.InOutBack).OnComplete(() => { gameObject.SetActive(false); });
    }
}