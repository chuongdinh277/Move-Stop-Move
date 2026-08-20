using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Root")]
    public Transform canvasParentTF;

    private Dictionary<System.Type, UICanvas> _uiPrefabs = new Dictionary<System.Type, UICanvas>();
    private Dictionary<System.Type, UICanvas> _instantiatedUIs = new Dictionary<System.Type, UICanvas>();
    private UICanvas[] _uiResources;

    #region Canvas Management

    public T OpenUI<T>() where T : UICanvas
    {
        UICanvas canvas = GetUI<T>();
        canvas.Setup();
        canvas.Open();
        return canvas as T;
    }

    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().CloseDirectly();
        }
    }

    public void CloseUI<T>(float delayTime) where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().Close(delayTime);
        }
    }

    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && _instantiatedUIs[typeof(T)].gameObject.activeInHierarchy;
    }

    public bool IsLoaded<T>() where T : UICanvas
    {
        System.Type type = typeof(T);
        return _instantiatedUIs.ContainsKey(type) && _instantiatedUIs[type] != null;
    }

    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            UICanvas canvas = Instantiate(GetUIPrefab<T>(), canvasParentTF);
            _instantiatedUIs[typeof(T)] = canvas;
        }
        return _instantiatedUIs[typeof(T)] as T;
    }

    public void CloseAll()
    {
        List<UICanvas> allCanvas = new List<UICanvas>(_instantiatedUIs.Values);
        foreach (var canvas in allCanvas)
        {
            if (canvas != null && canvas.gameObject.activeInHierarchy)
            {
                canvas.CloseDirectly();
            }
        }
    }

    private T GetUIPrefab<T>() where T : UICanvas
    {
        if (!_uiPrefabs.ContainsKey(typeof(T)))
        {
            if (_uiResources == null)
            {
                _uiResources = Resources.LoadAll<UICanvas>("UI/");
            }

            foreach (var ui in _uiResources)
            {
                if (ui is T)
                {
                    _uiPrefabs[typeof(T)] = ui;
                    break;
                }
            }
        }
        return _uiPrefabs[typeof(T)] as T;
    }

    #endregion

    #region Back Button Handling

    private Dictionary<UICanvas, UnityAction> _backActionEvents = new Dictionary<UICanvas, UnityAction>();
    private List<UICanvas> _backCanvasHistory = new List<UICanvas>();

    private UICanvas BackTopUI 
    {
        get
        {
            if (_backCanvasHistory.Count > 0)
            {
                return _backCanvasHistory[_backCanvasHistory.Count - 1];
            }
            return null;
        }
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && BackTopUI != null)
        {
            if (_backActionEvents.TryGetValue(BackTopUI, out UnityAction action))
            {
                action?.Invoke();
            }
        }
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!_backActionEvents.ContainsKey(canvas))
        {
            _backActionEvents.Add(canvas, action);
        }
    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!_backCanvasHistory.Contains(canvas))
        {
            _backCanvasHistory.Add(canvas);
        }
    }

    public void RemoveBackUI(UICanvas canvas)
    {
        _backCanvasHistory.Remove(canvas);
    }

    public void ClearBackKey()
    {
        _backCanvasHistory.Clear();
    }

    #endregion
}
