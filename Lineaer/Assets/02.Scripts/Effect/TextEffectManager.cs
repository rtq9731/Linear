using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffectManager : MonoSingleton<TextEffectManager>
{
    [SerializeField] TextEffect effectTextPrefab = null;

    Queue<TextEffect> textPool;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GetEffect();
        }
    }

    private TextEffect GetEffect()
    {
        TextEffect result = null;
        if(!textPool.Peek().gameObject.activeSelf)
        {
            result = Instantiate<TextEffect>(effectTextPrefab, transform);
            textPool.Enqueue(result);
        }
        else
        {
            result = textPool.Dequeue();
            textPool.Enqueue(result);
        }
        result.gameObject.SetActive(false);
        return result;
    }

    public void PlayDamageEffect(string str, Vector2 pos, Color color)
    {
        pos.y += 0.5f;
        GetEffect().InitTextEffect(str, pos, color);
    }
}