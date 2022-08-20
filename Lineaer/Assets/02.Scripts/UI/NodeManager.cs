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

    private int nowChapter = 0;

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

            isChapter3 = selects[y].result == 5;
            isEndingChapter = selects[y].result == END_CHAPTER;

            if (selects[i].isChapterSelect)
            {
                selectBtns[selects[i].idx].onClick.AddListener(() =>
                {
                    StateController.Instance.SetChapter3(isChapter3);
                    if (selects[y].result == 1 || selects[y].result == 2 || isChapter3)
                    {
                        FlowerController.Instance.SetFlowerLevel(++nowChapter);
                    }

                    curChapter = selects[y].result;
                    curChapter = Array.IndexOf(data.chapters.Select(item => item.idx).ToArray(), curChapter);
                    curSelectNum = 0;
                    data.chapters[curChapter].Shuffle();

                    TranslationManager.Instance.SelectToSelect(() =>
                    {
                        SetLayout();
                    });
                }); // ���� �б� �̺�Ʈ��� ��� é�ͷ� ������ üũ
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
                            TranslationManager.Instance.SelectToSelect(() =>
                            {
                                audioSource.Play();
                                SetLayout();
                            });
                        });

                        curSelectNum = 0;
                        FindObjectOfType<BGM>().StopBGM();
                    }
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

                    float effectOffSet = 0f;

                    StateController.Instance.AddStateValue(StateType.MONEY, selects[y].resultInfo.plusMoney);
                    if (selects[y].resultInfo.plusMoney != 0)
                    {
                        if (0 < selects[y].resultInfo.plusMoney)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�� ���!", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.green);
                        }
                        else if (selects[y].resultInfo.plusMoney < 0)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�� �϶�.", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.red);
                        }
                        effectOffSet += 128;
                    }

                    StateController.Instance.AddStateValue(StateType.MENTAL, selects[y].resultInfo.plusMental);
                    if (selects[y].resultInfo.plusMental != 0)
                    {
                        if (0 < selects[y].resultInfo.plusMental)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("���� ���!", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.green);
                        }
                        else if (selects[y].resultInfo.plusMental < 0)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("���� �϶�.", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.red);
                        }
                        effectOffSet += 128;
                    }

                    StateController.Instance.AddStateValue(StateType.HEALTH, selects[y].resultInfo.plusHealth);
                    if (selects[y].resultInfo.plusHealth != 0)
                    {
                        if (0 < selects[y].resultInfo.plusHealth)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�ǰ� ���!", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.green);
                        }
                        else if (selects[y].resultInfo.plusHealth < 0)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�ǰ� �϶�.", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.red);
                        }
                        effectOffSet += 128;
                    }

                    StateController.Instance.AddStateValue(StateType.WRITING, selects[y].resultInfo.plusWriting);
                    if (selects[y].resultInfo.plusWriting != 0)
                    {
                        if (0 < selects[y].resultInfo.plusWriting)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�ʷ� ���!", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.green);
                        }
                        else if (selects[y].resultInfo.plusWriting < 0)
                        {
                            TextEffectManager.Instance.PlayDamageEffect("�ʷ� �϶�.", (Vector2)Input.mousePosition + Vector2.right * effectOffSet, Color.red);
                        }
                        effectOffSet += 128;
                    }

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
