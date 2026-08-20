using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class IndicatorUI : GameUnit
{
    public RectTransform rectTransform; 
    public TextMeshProUGUI nameText;               
    public TextMeshProUGUI expText;                
    
    public Image expBackgroundImage;    
    
    public GameObject arrowObj;         
    public RectTransform arrowRect;     
    
    [HideInInspector] public int lastExp = -1; 
    [HideInInspector] public Color lastColor = Color.clear; 
}