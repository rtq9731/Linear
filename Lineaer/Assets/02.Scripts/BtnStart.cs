using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BtnStart : MonoBehaviour
{
    PlayableDirector pd = null;

    private void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        GetComponent<Button>().onClick.AddListener(() => 
        {
            pd.Play();
            GetComponent<Button>().onClick.RemoveAllListeners();
        });
    }

    public void OnCompeletePlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
}
