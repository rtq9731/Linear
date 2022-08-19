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
        stateValues[(int)stateType] += value;
        _stateImages[(int)stateType].fillAmount = stateValues[(int)stateType] / 100f;
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
}
