using UnityEngine;

[System.Serializable]
public class SetFullItemData
{
    public SetFullItemType setFullType;
    public string setFullName;

    public GameObject hatPrefab;
    public Material pantMat;
    public GameObject accessoryPrefab;
    public GameObject leftHandPrefab;
    public GameObject tailPrefab;

    public int cost;
}
