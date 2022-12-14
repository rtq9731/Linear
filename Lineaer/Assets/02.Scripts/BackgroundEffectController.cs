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
    }

    public void SetRisk(float risk)
    {
        canvasGroup.alpha = risk;
    }

    public void SetMedium()
    {
        medium = 0;
        float max = 0;
        for (int i = 0; i < (int)StateType.COUNT; ++i)
        {
            max = max < (Mathf.Abs(50 - StateController.Instance.GetStateValue((StateType)i))) ? Mathf.Abs(50 - StateController.Instance.GetStateValue((StateType)i)) : max;
        }
        medium = max;
    }
}
