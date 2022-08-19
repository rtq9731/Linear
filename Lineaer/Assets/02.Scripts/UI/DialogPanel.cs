using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] Text textDialog;
    [SerializeField] Image imageDialogComplete = null;
    [SerializeField] AudioClip[] clickSounds = null;
    [SerializeField] AudioSource audioSource = null;

    string strText = "";

    Coroutine routine = null;

    string[] dialogs = null;

    int i = 0;

    bool isSkip = false;

    public void SetDialog(string[] dialogs, System.Action onComplete)
    {
        this.dialogs = dialogs;
        i = 0;
        isSkip = false;
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(Dialog(onComplete));
    }

    private void Update()
    {
        if(routine != null && !isSkip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSkip = true;
            }
        }
    }

    private IEnumerator Dialog(string[] dialogs)
    {
        for (int i = 0; i < dialogs.Length; i++)
        {
            for (int j = 0; j < dialogs[i].Length; j++)
            {
                textDialog.text += dialogs[i][j];
                if (isSkip)
                {
                    isSkip = false;
                    textDialog.text = dialogs[i];
                    break;
                }
                audioSource.clip = clickSounds[Random.Range(0, clickSounds.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(0.1f);

                imageDialogComplete.gameObject.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                imageDialogComplete.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator Dialog(System.Action onComplete)
    {
        while (dialogs.Length > i)
        {
            int x = i;
            textDialog.text = "";

            for (int i = 0; i < dialogs[this.i].Length; i++)
            {
                textDialog.text += dialogs[this.i][i];
                if(isSkip)
                {
                    isSkip = false;
                    textDialog.text = dialogs[this.i];
                    break;
                }
                audioSource.clip = clickSounds[Random.Range(0, clickSounds.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(0.1f);
            }

            imageDialogComplete.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            imageDialogComplete.gameObject.SetActive(false);

            i++;
            isSkip = false;
        }

        onComplete?.Invoke();
    }
}
