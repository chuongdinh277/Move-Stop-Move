using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorManager : Singleton<IndicatorManager> 
{
    [Header("Settings")]
    public float edgePadding = 50f;     
    
    public static List<CharacterBase> targets = new List<CharacterBase>();
    
    private List<IndicatorUI> pool = new List<IndicatorUI>();
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        ManagePool(targets.Count);

        for (int i = 0; i < pool.Count; i++)
        {
            UpdateIndicator(pool[i], targets[i]);
        }
    }
    private void ManagePool(int targetCount)
    {
        while (pool.Count < targetCount)
        {
            IndicatorUI obj = SimplePool.Spawn<IndicatorUI>(PoolType.NameTag, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(transform, false);
            pool.Add(obj);
        }

        while (pool.Count > targetCount)
        {
            int lastIndex = pool.Count - 1;
            IndicatorUI obj = pool[lastIndex];
            pool.RemoveAt(lastIndex);
            
            SimplePool.Despawn(obj);
        }
    }

    private void UpdateIndicator(IndicatorUI ui, CharacterBase target)
    {
        UpdatePositionAndArrow(ui, target);
        UpdateUIInfo(ui, target);
    }

    private void UpdatePositionAndArrow(IndicatorUI ui, CharacterBase target)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.TF.position + Vector3.up * 2f);
        bool isOffScreen = screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffScreen)
        {
            SetOffScreenTransform(ui, screenPos);
        }
        else
        {
            SetOnScreenTransform(ui, screenPos);
        }
    }

    private void SetOffScreenTransform(IndicatorUI ui, Vector3 screenPos)
    {
        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 dir = screenPos - center;
        
        if (screenPos.z < 0)
        {
            dir *= -1;
        }
        dir = dir.normalized; 
        
        Vector3 finalPos = center + dir * 2000f; 
        finalPos.x = Mathf.Clamp(finalPos.x, edgePadding, Screen.width - edgePadding);
        finalPos.y = Mathf.Clamp(finalPos.y, edgePadding, Screen.height - edgePadding);
        
        ui.rectTransform.position = finalPos;
        
        ui.arrowObj.SetActive(true);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ui.arrowRect.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SetOnScreenTransform(IndicatorUI ui, Vector3 screenPos)
    {
        ui.rectTransform.position = screenPos;
        ui.arrowObj.SetActive(false); 
    }

    private void UpdateUIInfo(IndicatorUI ui, CharacterBase target)
    {
        int currentExp = target.GetCurrentExp(); 
        
        if (ui.lastExp != currentExp)
        {
            ui.lastExp = currentExp;
            ui.expText.text = currentExp.ToString();
            ui.nameText.text = target.gameObject.name; 
        }
        
        if (target.GetBodyMeshRenderer() != null)
        {
            Color charColor = target.GetBodyMeshRenderer().material.color;
            if (ui.lastColor != charColor)
            {
                ui.lastColor = charColor;
                
                if (ui.nameText != null) ui.nameText.color = charColor;
                if (ui.expBackgroundImage != null) ui.expBackgroundImage.color = charColor;
                
                Image arrowImg = ui.arrowObj.GetComponent<Image>();
                if (arrowImg != null) arrowImg.color = charColor;
            }
        }
    }
}