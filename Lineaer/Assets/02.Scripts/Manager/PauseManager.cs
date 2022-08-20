using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoSingleton<PauseManager>
{
    public bool isPaused { get; private set; } = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        isPaused = Time.timeScale == 0;
        transform.GetChild(0).gameObject.SetActive(isPaused);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
