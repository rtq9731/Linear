using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using DG.Tweening;

public class EndingController : MonoSingleton<EndingController>
{
    [SerializeField] GameObject[] removeObjs = null;
    [SerializeField] GameObject[] activeObjs = null;
    [SerializeField] Text textEnding;

    [SerializeField] AudioSource clickSource = null;
    [SerializeField] AudioClip[] clickSounds = null;

    int clickNum = 0;

    string[] dialogs =
    {
        "그렇게 셰익스피어는 모두에게 인정받는 대작가로서 생을 마쳤습니다.",
        "그의 사후 그의 작품들은 재발굴되어 그는 영문학계의 북극성으로서 우뚝 군림할 수 있게 되었습니다.",
        "셰익스피어, 그 위대한 대문호의 이름을 우리는 다시 한 번 불러봅니다…",
        "윌리엄 셰익스피어!"
    };

    private void PlayClickSound()
    {
        clickSource.clip = clickSounds[clickNum % clickSounds.Length];
        clickNum++;
    }

    public void Ending()
    {
        ScreenFader.Instance.ScreenFade(1f, 0, () => {
            foreach (var item in removeObjs)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var item in activeObjs)
            {
                item.gameObject.SetActive(true);
            }
        },
        () => StartCoroutine(EndingCorutine()));
    }

    private IEnumerator EndingCorutine()
    {
        textEnding.text = "";
        yield return null;
        for (int i = 0; i < dialogs.Length; i++)
        {
            for (int j = 0; j < dialogs[i].Length; j++)
            {
                textEnding.text += dialogs[i][j];
                PlayClickSound();
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2f);

            while (textEnding.text.Length >= 1)
            {
                textEnding.text.Remove(textEnding.text.Length - 1);
                yield return new WaitForSeconds(0.075f);
            }

            if (i < dialogs.Length - 1)
            {
                textEnding.text = "";
            }
        }

        textEnding.DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene1");
    }
}
