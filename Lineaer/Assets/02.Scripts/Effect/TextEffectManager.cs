using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffectManager : MonoSingleton<TextEffectManager>
{
    [SerializeField] TextEffect effectTextPrefab = null;

    Queue<TextEffect> textPool = new Queue<TextEffect>();

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            TextEffect effect = Instantiate<TextEffect>(effectTextPrefab, transform);
            effect.gameObject.SetActive(false);
            textPool.Enqueue(effect);
        }
    }

    private TextEffect GetEffect()
    {
        TextEffect result = null;
        if(textPool.Count > 0)
        {
            if (!textPool.Peek().gameObject.activeSelf)
            {
                result = Instantiate<TextEffect>(effectTextPrefab, transform);
                textPool.Enqueue(result);
            }
            else
            {
                result = textPool.Dequeue();
                textPool.Enqueue(result);
            }
        }
        else
        {
            result = Instantiate<TextEffect>(effectTextPrefab, transform);
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