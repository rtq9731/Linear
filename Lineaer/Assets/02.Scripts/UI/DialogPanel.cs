using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] Text textDialog;
    [SerializeField] Text[] textUpperDialog;
    [SerializeField] Image imageTalker = null;
    [SerializeField] Image imageDialogComplete = null;
    [SerializeField] AudioClip[] clickSounds = null;
    [SerializeField] AudioSource audioSource = null;

    Coroutine routine = null;

    Sprite[] sprites = null;
    string[] dialogs = null;
    string[] upperDialogs = null;

    int dialogNum = 0;

    bool isSkip = false;
    bool isUpperDialog = false;

    public void SetDialog(Sprite[] sprites, string[] dialogs, string[] upperDialogs, System.Action onComplete)
    {
        this.dialogs = dialogs;
        this.upperDialogs = upperDialogs;
        this.sprites = sprites;
        dialogNum = 0;
        isSkip = false;
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(Dialog(onComplete));
    }

    public void SetDialog()
    {
        StartCoroutine(Dialog(() => { }));
    }

    private void Update()
    {
        if (routine != null && !isSkip)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSkip = true;
            }
        }
    }

    private IEnumerator Dialog(System.Action onComplete)
    {
        while (dialogs.Length > dialogNum)
        {
            int x = dialogNum;
            textDialog.text = "";
            for (int i = 0; i < textUpperDialog.Length; i++)
            {
                textUpperDialog[i].text = "";
            }

            int spriteIdx = sprites.Length <= dialogNum ? sprites.Length - 1 : dialogNum;

            imageTalker.sprite = sprites[spriteIdx];


            imageTalker.rectTransform.sizeDelta = sprites[spriteIdx].rect.size / 2f;
            StartCoroutine(SetUpperText(upperDialogs[dialogNum]));
            for (int i = 0; i < dialogs[dialogNum].Length; i++)
            {
                textDialog.text += dialogs[dialogNum][i];
                if (isSkip)
                {
                    isSkip = false;
                    textDialog.text = dialogs[dialogNum];
                    break;
                }
                audioSource.clip = clickSounds[Random.Range(0, clickSounds.Length)];
                audioSource.Play();
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitWhile(() => isUpperDialog);

            imageDialogComplete.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            imageDialogComplete.gameObject.SetActive(false);

            dialogNum++;
            isSkip = false;
        }

        onComplete?.Invoke();
    }

    private IEnumerator SetUpperText(string text)
    {
        isUpperDialog = true;
        int line = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n') // ���� �ٹٲ��� �´ٸ�
            {
                line++;
                i++;
            }

            textUpperDialog[line].text += text[i];

            if (isSkip)
            {
                isSkip = false;
                for (int j = 0; j < text.Length; j++)
                {
                    if (text[j] == '\n') // ���� �ٹٲ��� �´ٸ�
                    {
                        line++;
                    }

                    textUpperDialog[line].text += text[j];
                }
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        isUpperDialog = false;
    }
}
