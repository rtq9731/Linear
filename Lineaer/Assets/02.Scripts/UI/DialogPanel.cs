using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] Text textDialog;
    [SerializeField] Image imageDialogComplete = null;

    Coroutine routine = null;

    string[] dialogs = null;

    int dialogNum = 0;

    public void SetDialog(string[] dialogs, System.Action onComplete)
    {
        this.dialogs = dialogs;
        dialogNum = 0;
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(Dialog(onComplete));
    }

    private IEnumerator Dialog(System.Action onComplete)
    {
        while (dialogs.Length > dialogNum)
        {
            int x = dialogNum;
            textDialog.text = "";

            for (int i = 0; i < dialogs[dialogNum].Length; i++)
            {
                textDialog.text += dialogs[dialogNum][i];
                yield return new WaitForSeconds(0.1f);
            }

            imageDialogComplete.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            imageDialogComplete.gameObject.SetActive(false);
            dialogNum++;
        }

        onComplete?.Invoke();
    }
}
