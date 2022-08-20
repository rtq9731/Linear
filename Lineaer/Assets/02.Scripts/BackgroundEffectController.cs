using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEffectController : MonoSingleton<BackgroundEffectController>
{
    private CanvasGroup canvasGroup = null;

    private float medium = 0;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void AddRisk()
    {
        canvasGroup.alpha = medium / 50f;
        Debug.Log("AddRisk " + medium / 50f);
    }

    public void SetMedium()
    {
        medium = 0;
        for (int i = 0; i < (int)StateType.COUNT; ++i)
        {
            medium += (Mathf.Abs(50 - StateController.Instance.GetStateValue((StateType)i)));
        }
        medium /= (int)StateType.COUNT;
    }
}
