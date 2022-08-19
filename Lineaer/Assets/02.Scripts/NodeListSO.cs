using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/NodeList", fileName = "NodeListSO")]
public class NodeListSO : ScriptableObject
{
    public List<NodeInfo> nodes = null;
}

[System.Serializable]
public class NodeInfo
{
    public int idx = 0;
    public string[] dialogs = null;
    public Sprite talkerSprite = null;
    public SelectInfo[] selects = null;
}

[System.Serializable]
public class SelectInfo
{
    public bool isChapterSelect = false;
    public bool isEndSelect = false;
    public int idx = 0;
    public string selectInfo;
    public int result = 0;
}

