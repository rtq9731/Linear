using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/NodeList", fileName = "NodeListSO")]
public class ChapterListSO : ScriptableObject
{
    public List<ChapterInfo> chapters = null;
}

[System.Serializable]
public class ChapterInfo
{
    public int idx = 0;
    public List<NodeInfo> nodes = new List<NodeInfo>(5); // 챕터 페이지 수
    public NodeInfo endNode = null;
    public int pageCnt = 5;
    public void Shuffle()
    {
        System.Random rng = new System.Random();
        var shufflednodes = nodes.OrderBy(a => rng.Next()).ToList();
        nodes = shufflednodes;
    }
}


[System.Serializable]
public class NodeInfo
{
    public int idx = 0;
    [TextArea] public string[] dialogs = null;
    [TextArea] public string[] upperDialog = null;
    public Sprite[] sprites = null;
    public SelectInfo[] selects = null;
}

[System.Serializable]
public class SelectInfo
{
    public bool isChapterSelect = false;
    public bool isEndSelect = false;
    public bool isRestartSelect = false;
    public int idx = 0;
    public string selectInfo;
    public string selectInfoUpper = "";
    public int result = 0;
    public ResultInfo resultInfo = null;
}

[System.Serializable]
public class ResultInfo
{
    public int plusMoney = 0;
    public int plusMental = 0;
    public int plusWriting = 0;
    public int plusHealth = 0;
}


