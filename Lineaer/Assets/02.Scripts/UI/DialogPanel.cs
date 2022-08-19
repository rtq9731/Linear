using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] Text textDialog;
    [SerializeField] Image imageDialogComplete = null;

    string[] dialogs = null;

    int dialogNum = 0;

    public void SetDialog(string[] dialogs, System.Action onComplete)
    {
        this.dialogs = dialogs;
        dialogNum = 0;
        StartCoroutine(Dialog(onComplete));
    }

    private IEnumerator Dialog(System.Action onComplete)
    {
        while (dialogs.Length < dialogNum)
        {
            Tween tween = textDialog.DOText(dialogs[dialogNum], dialogs[dialogNum].Length * 0.25f);
            yield return new WaitUntil(tween.IsComplete);
            imageDialogComplete.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            imageDialogComplete.gameObject.SetActive(false);
            dialogNum++;
        }

        onComplete.Invoke();
    }
}
