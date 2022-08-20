using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BtnStart : MonoBehaviour
{
    PlayableDirector pd = null;

    private void Awake()
    {
        pd = GetComponent<PlayableDirector>();
    }

    public void OnCompeletePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
