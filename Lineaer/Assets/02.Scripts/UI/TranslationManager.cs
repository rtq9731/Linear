using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TranslationManager : MonoSingleton<TranslationManager>
{
    [SerializeField] LetterUI letter;
    [SerializeField] PlayableDirector pd = null;

    [SerializeField] PlayableAsset[] playableClips = null;

    Action onMiddleOfTransition;

    public void SelectToSelect(Action onChange)
    {
        onMiddleOfTransition = () => { };
        onMiddleOfTransition += onChange;
        onMiddleOfTransition += () => onChange -= onMiddleOfTransition;

        pd.playableAsset = playableClips[0];
        pd.Play();
    }

    public void SelectToEnding(NodeInfo endNode)
    {
        onMiddleOfTransition = () => { };
        onMiddleOfTransition += () =>
        {
            letter.SetLetter(endNode);
        };

        pd.playableAsset = playableClips[1];
        pd.Play();
    }

    public void OnMiddle()
    {
        onMiddleOfTransition?.Invoke();
    }
}
