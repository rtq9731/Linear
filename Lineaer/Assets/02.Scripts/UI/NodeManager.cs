using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance => _instance;
    private static NodeManager _instance = null;

    readonly int chapterPage = 5;

    [SerializeField] ChapterListSO data = null;
    [SerializeField] DialogPanel dialogPanel = null;

    [SerializeField] Text textdialog = null;

    [SerializeField] Button[] selectBtns = null;

    [SerializeField] RectTransform[] endLayout = null;

    [SerializeField] RectTransform mainLayout = null;

    int curChapter = 0;
    int curSelectNum = 0;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }

    public void SetLayout(NodeInfo nodeInfo)
    {
        SetBtns(nodeInfo.selects);
        dialogPanel.SetDialog(nodeInfo.dialogs);
    }

    public void SetLayout()
    {
        NodeInfo nodeInfo = data.chapters[chapterPage].nodes[curSelectNum];

        SetBtns(nodeInfo.selects);
    }

    public void SetEndLayout(int endNum)
    {
        endLayout[endNum].gameObject.SetActive(true);
        mainLayout.gameObject.SetActive(false);
    }

    public void SetBtns(SelectInfo[] selects)
    {
        for (int i = 0; i < selectBtns.Length; i++)
        {
            selectBtns[i].onClick.RemoveAllListeners(); // ��� ��ư �ʱ�ȭ
        }

        for (int i = 0; i < selects.Length; i++)
        {
            if (selects[i].isChapterSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() => 
                {
                    curChapter = selects[i].result;
                    curSelectNum = 0;
                    SetLayout();
                }); // ���� �б� �̺�Ʈ��� ��� é�ͷ� ������ üũ
            }
            else if (selects[i].isEndSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    SetEndLayout(selects[i].result);
                }); // ���� ���� �̺�Ʈ��� ���� ȭ�� �ҷ�����
            }
            else
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curSelectNum++;
                    // TODO : ���⼭ ��ġ �����ϱ�
                    SetLayout();
                }); // ���� �б� �̺�Ʈ�� �ƴ϶�� ���� �������� �׳� �Ѿ��
            }
        }
    }
}
