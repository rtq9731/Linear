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

    [SerializeField] Vector2[] originBtns = new Vector2[3];

    int curChapter = 0;
    int curSelectNum = 0;

    private void Start()
    {
        for (int i = 0; i < selectBtns.Length; i++)
        {
            originBtns[i] = selectBtns[i].GetComponent<RectTransform>().anchoredPosition;
            selectBtns[i].gameObject.SetActive(false);
        }
        SetLayout();
    }

    public void SetLayout()
    {
        NodeInfo nodeInfo = data.chapters[curChapter].nodes[curSelectNum];
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
            selectBtns[i].onClick.RemoveAllListeners(); // 모든 버튼 초기화
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
                }); // 만약 분기 이벤트라면 몇번 챕터로 가는지 체크
            }
            else if (selects[i].isEndSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    SetEndLayout(selects[i].result);
                }); // 만약 엔딩 이벤트라면 엔딩 화면 불러오기
            }
            else
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curSelectNum++;
                    // TODO : 여기서 수치 조정하기
                    SetLayout();
                }); // 만약 분기 이벤트가 아니라면 다음 선택지로 그냥 넘어가게
            }
        }
    }
}
