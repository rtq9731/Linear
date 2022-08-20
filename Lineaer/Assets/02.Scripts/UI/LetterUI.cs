using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterUI : MonoBehaviour
{
    [SerializeField] Text textMain = null;
    [SerializeField] Text[] upperTexts = null;

    public void SetLetter(NodeInfo node)
    {
        string mainStr = "";
        for (int i = 0; i < node.dialogs.Length; i++)
        {
            mainStr += node.dialogs[i];
            mainStr += '\n';
            upperTexts[i].text = node.upperDialog[i];
        }
        textMain.text = mainStr;
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene1");
        }
    }
}
