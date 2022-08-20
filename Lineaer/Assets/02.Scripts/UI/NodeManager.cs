using System;
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
    [SerializeField] Image bgImage = null;

    [SerializeField] Text textdialog = null;

    [SerializeField] Button[] selectBtns = null;

    [SerializeField] AudioSource audioSource = null;

    Vector2[] btnOriginPos = new Vector2[3];

    public const int END_CHAPTER = 1616;

    private bool isEndingChapter = false;
    private bool isChapter3 = false;
    private bool buttonMoveComplete = false;

    public DialogPanel DialogPanel { get { return dialogPanel; } }

    public int CurChapter
    {
        set
        {
            curChapter = value;
        }
    }

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
        NodeInfo nodeInfo = null;
        if (curSelectNum >= data.chapters.Find(item => item.idx == curChapter).pageCnt)
        {
            nodeInfo = data.chapters.Find(item => item.idx == curChapter).endNode;
        }
        else
        {
            nodeInfo = data.chapters.Find(item => item.idx == curChapter).nodes[curSelectNum];
        }

        for (int i = 0; i < selectBtns.Length; i++)
        {
            selectBtns[i].onClick.RemoveAllListeners(); // 모든 버튼 초기화
            selectBtns[i].GetComponentsInChildren<Text>()[1].text = "";
            selectBtns[i].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 2000; // 위치도 오른쪽으로 숨김
            selectBtns[i].interactable = false; // 버튼 상호작용 없애기
            selectBtns[i].gameObject.SetActive(false); // 버튼 끄기
        }

        // bgImage.sprite = nodeInfo.bgSprites;
        dialogPanel.SetDialog(nodeInfo.sprites, nodeInfo.dialogs, nodeInfo.upperDialog, () => SetBtns(nodeInfo.selects));
    }

    public void SetBtns(SelectInfo[] selects)
    {
        for (int i = 0; i < selects.Length; i++)
        {
            int y = i;

            isChapter3 = selects[y].result == 5;
            isEndingChapter = selects[y].result == END_CHAPTER;

            if (selects[i].isChapterSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    StateController.Instance.SetChapter3(isChapter3);
                    curChapter = selects[y].result;
                    curChapter = Array.IndexOf(data.chapters.Select(item => item.idx).ToArray(), curChapter);
                    curSelectNum = 0;
                    data.chapters[curChapter].Shuffle();
                    SetLayout();
                }); // 만약 분기 이벤트라면 몇번 챕터로 가는지 체크
            }
            else if (selects[i].isEndSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    if (isEndingChapter)
                    {
                        EndingController.Instance.Ending();
                        return;
                    }
                    else
                    {
                        curChapter = selects[y].result;
                        ScreenFader.Instance.ScreenFade(2f, audioSource.clip.length, () =>
                        {
                            audioSource.Play();
                            SetLayout();
                        });

                        curSelectNum = 0;
                        FindObjectOfType<BGM>().StopBGM();
                    }
                }); // 만약 엔딩 이벤트라면 엔딩 화면 불러오기
            }
            else if (selects[i].isRestartSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene1"));
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
            if (selects[i].selectInfoUpper != "")
            {
                Text textInfoUpper = selectBtns[selects[i].idx].GetComponentsInChildren<Text>()[1];
                textInfoUpper.text = selects[i].selectInfoUpper;
                textInfoUpper.gameObject.SetActive(true);
            }

        }

        Sequence seq = DOTween.Sequence();
        StartCoroutine(MouseSensor());
        for (int i = 0; i < selects.Length; i++)
        {
            selectBtns[i].gameObject.SetActive(true);
            int y = i;
            buttonMoveComplete = false;
            seq.Append(selectBtns[i].GetComponent<RectTransform>().DOAnchorPos(btnOriginPos[i], 0.5f).SetEase(Ease.InOutBack));
            seq.AppendInterval(0.125f);
        }
        seq.OnComplete(() =>
        {
            selectBtns.ToList().ForEach(item => item.interactable = true);
            buttonMoveComplete = true;
        });
        seq.Play();
    }

    private IEnumerator MouseSensor()
    {
        yield return new WaitForSeconds(0.1f);
        while (!buttonMoveComplete)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DOTween.KillAll();
                selectBtns.ToList().ForEach(item => item.interactable = true);
                buttonMoveComplete = true;
                for (int i = 0; i < 3; ++i)
                {
                    selectBtns[i].GetComponent<RectTransform>().DOAnchorPos(btnOriginPos[i], 0f);
                }
                break;
            }
            yield return null;
        }
    }
}
