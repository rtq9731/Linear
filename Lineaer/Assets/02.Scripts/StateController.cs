using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateController : MonoSingleton<StateController>
{
    private float[] stateValues;

    [SerializeField]
    private Image[] _stateImages;

    private const float MAX_VALUE = 100f;
    private const float MIN_VALUE = 0f;

    private bool _isChapter3 = false;

    private void Awake()
    {
        stateValues = new float[(int)StateType.COUNT];
    }

    private void Start()
    {
        for (int i = 0; i < stateValues.Length; i++)
        {
            SetStateValue((StateType)i, 50.0f);
        }
    }

    /// <summary>
    /// Sets the value of a state.
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="value"></param>
    public void SetStateValue(StateType stateType, float value)
    {
        stateValues[(int)stateType] = value;
        _stateImages[(int)stateType].fillAmount = stateValues[(int)stateType] / 100f;

    }

    /// <summary>
    /// Add the value of a state.
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="value"></param>
    public void AddStateValue(StateType stateType, float value)
    {
        ArrowController.Instance.SetArrow(stateType, value);
        if (0 < value)
        {
            _stateImages[(int)stateType].gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0.1222707f, 0.4716981f, 0.07787468f, 1f);
        }
        else if (value < 0)
        {
            _stateImages[(int)stateType].gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0.4705882f, 0.07843135f, 0.108476f, 1f);
        }
        else
        {
            return;
        }

        stateValues[(int)stateType] += value;


        if (_isChapter3)
        {
            stateValues[(int)stateType] = Mathf.Clamp(stateValues[(int)stateType], MIN_VALUE + 1, MAX_VALUE - 1);
        }

        BackgroundEffectController.Instance.SetMedium();
        BackgroundEffectController.Instance.AddRisk();

        CheckedStateLimited(stateType);
        StartCoroutine(ChangeStateValue((StateType)stateType, stateValues[(int)stateType]));
    }

    /// <summary>
    /// Get the value of a state
    /// </summary>
    /// <param name="stateType"></param>
    /// <returns></returns>
    public float GetStateValue(StateType stateType)
    {
        return stateValues[(int)stateType];
    }

    private void CheckedStateLimited(StateType stateType)
    {
        if (stateValues[(int)stateType] < MAX_VALUE && stateValues[(int)stateType] > MIN_VALUE)
        {
            return;
        }
        else if (stateType == StateType.HEALTH && stateValues[(int)stateType] >= MAX_VALUE)
        {
            return;
        }

        bool excess = stateValues[(int)stateType] >= MAX_VALUE;
        int endingIdx = excess ? 1 : 2;

        BackgroundEffectController.Instance.SetRisk(1f);

        switch (stateType)
        {
            case StateType.HEALTH:
                endingIdx = 107;
                break;
            case StateType.MONEY:
                endingIdx += 100;
                break;
            case StateType.MENTAL:
                endingIdx += 102;
                break;
            case StateType.WRITING:
                endingIdx += 104;
                break;
            default:
                Debug.LogWarning("type is not defined");
                break;
        }

        NodeManager.Instance.CurChapter = endingIdx;
        NodeManager.Instance.SetLayout();
    }

    private IEnumerator ChangeStateValue(StateType stateType, float value, float duration = 1.25f)
    {
        float startValue = _stateImages[(int)stateType].fillAmount;
        float endValue = value / 100f;
        float time = 0f;
        while (time < duration)
        {
            _stateImages[(int)stateType].fillAmount = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _stateImages[(int)stateType].fillAmount = endValue;
        yield return new WaitForSeconds(0.25f);
        _stateImages[(int)stateType].gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void SetChapter3(bool isChapter3)
    {
        _isChapter3 = isChapter3;
    }
}
