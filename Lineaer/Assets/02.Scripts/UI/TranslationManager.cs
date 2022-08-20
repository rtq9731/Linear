using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TranslationManager : MonoSingleton<TranslationManager>
{
    [SerializeField] PlayableDirector pd = null;

    [SerializeField] PlayableAsset[] playableClips = null;

    Action onMiddleOfTransition;

    public void SelectToSelect(Action onChange)
    {
        onMiddleOfTransition += onChange;
        onMiddleOfTransition += () => onChange -= onMiddleOfTransition;

        pd.playableAsset = playableClips[0];
        pd.Play();
    }

    public void OnMiddle()
    {
        onMiddleOfTransition?.Invoke();
    }
}
