using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData_Config", menuName = "ScriptableObjects/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    [Header("Color Configurations")]
    public List<ColorItemData> colorList;
    public Material GetColorMat(ColorType type)
    {
        foreach (ColorItemData item in colorList)
        {
            if (item.colorType == type)
            {
                return item.colorMat;
            }
        }
        return null;
    }
}
