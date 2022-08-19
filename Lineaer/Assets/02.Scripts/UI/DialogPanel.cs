using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogPanel : MonoBehaviour
{
    [SerializeField] Text textDialog;

    string[] dialogs = null;

    public void SetDialog(string[] dialogs)
    {
        this.dialogs = dialogs;
    }
}
