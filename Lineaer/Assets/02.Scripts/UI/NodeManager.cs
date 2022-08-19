using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NodeManager : MonoSingleton<NodeManager>
{
    [SerializeField] ChapterListSO data = null;
    [SerializeField] DialogPanel dialogPanel = null;

    [SerializeField] Text textdialog = null;

    [SerializeField] Button[] selectBtns = null;

    [SerializeField] RectTransform[] endLayout = null;

    [SerializeField] RectTransform mainLayout = null;

    Vector2[] btnOriginPos = new Vector2[3];

    int curChapter = 0;
    int curSelectNum = 0;

    private void Start()
    {
        for (int i = 0; i < selectBtns.Length; i++)
        {
            btnOriginPos[i] = selectBtns[i].GetComponent<RectTransform>().anchoredPosition;
            selectBtns[i].gameObject.SetActive(false);
        }
        SetLayout();
    }

    public void SetLayout()
    {
        NodeInfo nodeInfo = data.chapters[curChapter].nodes[curSelectNum];
        if(curSelectNum >= 5)
        {
            nodeInfo = data.chapters[curChapter].endNode;
        }
        dialogPanel.SetDialog(nodeInfo.dialogs, () => SetBtns(nodeInfo.selects));
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
            selectBtns[i].GetComponent<RectTransform>().anchoredPosition = Vector2.right * 2000; // ��ġ�� ���������� ����
            selectBtns[i].gameObject.SetActive(false);
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

        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < selects.Length; i++)
        {
            seq.Append(selectBtns[i].GetComponent<RectTransform>().DOAnchorPosX(btnOriginPos[i].x, 1f).SetEase(Ease.InOutBack));
            seq.AppendInterval(0.125f);
        }
    }
}
