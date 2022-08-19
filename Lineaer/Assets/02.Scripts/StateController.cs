using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateController : MonoSingleton<StateController>
{
    private float[] stateValues;

    [SerializeField]
    private Image[] _stateImages;

    private void Awake()
    {
        stateValues = new float[(int)StateType.COUNT];
        _stateImages = new Image[(int)StateType.COUNT];
    }

    /// <summary>
    /// Sets the value of a state.
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="value"></param>
    public void SetStateValue(StateType stateType, float value)
    {
        _stateImages[(int)stateType].fillAmount = value;
    }

    /// <summary>
    /// Add the value of a state.
    /// </summary>
    /// <param name="stateType"></param>
    /// <param name="value"></param>
    public void AddStateValue(StateType stateType, float value)
    {
        stateValues[(int)stateType] += value;
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
