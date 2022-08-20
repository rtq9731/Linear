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
            selectBtns[i].onClick.RemoveAllListeners(); // ��� ��ư �ʱ�ȭ
            selectBtns[i].GetComponentsInChildren<Text>()[1].text = "";
            selectBtns[i].GetComponent<RectTransform>().anchoredPosition += Vector2.right * 2000; // ��ġ�� ���������� ����
            selectBtns[i].interactable = false; // ��ư ��ȣ�ۿ� ���ֱ�
            selectBtns[i].gameObject.SetActive(false); // ��ư ����
        }

        // bgImage.sprite = nodeInfo.bgSprites;
        dialogPanel.SetDialog(nodeInfo.sprites, nodeInfo.dialogs, nodeInfo.upperDialog, () => SetBtns(nodeInfo.selects));
    }

    public void SetBtns(SelectInfo[] selects)
    {
        for (int i = 0; i < selects.Length; i++)
        {
            int y = i;
            if (selects[y].result == 5)
            {
                StateController.Instance.SetChapter3(true);
            }

            if (selects[y].result == END_CHAPTER)
            {
                EndingController.Instance.Ending();
                return;
            }
            else if (selects[i].isChapterSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curChapter = selects[y].result;
                    curChapter = Array.IndexOf(data.chapters.Select(item => item.idx).ToArray(), curChapter);
                    curSelectNum = 0;
                    data.chapters[curChapter].Shuffle();
                    SetLayout();
                }); // ���� �б� �̺�Ʈ��� ��� é�ͷ� ������ üũ
            }
            else if (selects[i].isEndSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    curChapter = selects[y].result;
                    ScreenFader.Instance.ScreenFade(2f, audioSource.clip.length, () =>
                    {
                        audioSource.Play();
                        SetLayout();
                    });

                    curSelectNum = 0;
                    FindObjectOfType<BGM>().StopBGM();
                }); // ���� ���� �̺�Ʈ��� ���� ȭ�� �ҷ�����
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
                }); // ���� �б� �̺�Ʈ�� �ƴ϶�� ���� �������� �׳� �Ѿ��
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
