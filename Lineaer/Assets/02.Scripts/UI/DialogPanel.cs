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
            textDialog.text = "";
            Tween tween = textDialog.DOText(dialogs[dialogNum], dialogs[dialogNum].Length * 0.1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(dialogs[dialogNum].Length * 0.1f);
            Debug.Log(dialogs[dialogNum]);
            imageDialogComplete.gameObject.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            imageDialogComplete.gameObject.SetActive(false);
            dialogNum++;
        }

        onComplete?.Invoke();
    }
}
