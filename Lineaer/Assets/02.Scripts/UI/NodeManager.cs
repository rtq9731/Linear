using System.Linq;
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
            Debug.Log(btnOriginPos[i]);
            selectBtns[i].gameObject.SetActive(false);
        }
        SetLayout();
    }

    public void SetLayout()
    {
        NodeInfo nodeInfo = null;
        if (curSelectNum >= data.chapters[curChapter].pageCnt)
        {
            nodeInfo = data.chapters[curChapter].endNode;
        }
        else
        {
            nodeInfo = data.chapters[curChapter].nodes[curSelectNum];
        }

        for (int i = 0; i < selectBtns.Length; i++)
        {
            selectBtns[i].onClick.RemoveAllListeners(); // 모든 버튼 초기화
            selectBtns[i].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 2000; // 위치도 오른쪽으로 숨김
            selectBtns[i].interactable = false; // 버튼 상호작용 없애기
            selectBtns[i].gameObject.SetActive(false); // 버튼 끄기
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
        for (int i = 0; i < selects.Length; i++)
        {
            int y = i;
            if (selects[i].isChapterSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curChapter = selects[y].result;
                    curSelectNum = 0;
                    data.chapters[curChapter].Shuffle();
                    SetLayout();
                }); // 만약 분기 이벤트라면 몇번 챕터로 가는지 체크
            }
            else if (selects[i].isEndSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    SetEndLayout(selects[y].result);
                }); // 만약 엔딩 이벤트라면 엔딩 화면 불러오기
            }
            else
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curSelectNum++;
                    StateController.Instance.AddStateValue(StateType.MONEY, selects[y].resultInfo.plusMoney);
                    StateController.Instance.AddStateValue(StateType.MENTAL, selects[y].resultInfo.plusMental);
                    StateController.Instance.AddStateValue(StateType.HEALTH, selects[y].resultInfo.plusHealth);
                    StateController.Instance.AddStateValue(StateType.WRITING, selects[y].resultInfo.plusWriting);
                    SetLayout();
                }); // 만약 분기 이벤트가 아니라면 다음 선택지로 그냥 넘어가게
            }

            selectBtns[selects[i].idx].GetComponentInChildren<Text>().text = selects[i].selectInfo;

        }

        Sequence seq = DOTween.Sequence();
        for (int i = 0; i < selects.Length; i++)
        {
            selectBtns[i].gameObject.SetActive(true);
            int y = i;
            seq.Append(selectBtns[i].GetComponent<RectTransform>().DOAnchorPos(btnOriginPos[i], 0.5f).SetEase(Ease.InOutBack));
            seq.AppendInterval(0.125f);
        }
        seq.OnComplete(() => selectBtns.ToList().ForEach(item => item.interactable = true));
        seq.Play();
    }
}
