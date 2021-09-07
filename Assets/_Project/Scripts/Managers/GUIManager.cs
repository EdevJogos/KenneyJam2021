using UnityEngine;
using System;

public class GUIManager : MonoBehaviour
{
    public event Action onPlayRequested;
    public event Action onExitMatchRequested;
    public event Action onUnpauseRequested;

    public Canvas mainCanvas;
    public Display[] displays;

    [SerializeField] private Display _activeDisplay;

    public void Initiate()
    {
        Display.onActionRequested += OnActionRequested;

        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initiate();
        }
    }

    private void OnDestroy()
    {
        Display.onActionRequested -= OnActionRequested;
    }

    public void Initialize()
    {
        for (int __i = 0; __i < displays.Length; __i++)
        {
            displays[__i].Initialize();
        }
    }

    public void ShowDisplay(Displays p_display, float p_hideRatio = 1f, Action p_onShowCompleted = null, float p_showRatio = 1f)
    {
        if (_activeDisplay == null || (_activeDisplay != null && _activeDisplay.ID != p_display))
        {
            if (_activeDisplay != null)
            {
                _activeDisplay.Show(false, () => { ActiveDisplay(p_display, p_onShowCompleted, p_showRatio); }, p_hideRatio);
            }
            else
            {
                ActiveDisplay(p_display, p_onShowCompleted, p_showRatio);
            }
        }
    }

    private void ActiveDisplay(Displays p_display, Action p_onShowCompleted, float p_showRatio)
    {
        _activeDisplay = displays[(byte)p_display];
        _activeDisplay.Show(true, p_onShowCompleted, p_showRatio);
    }

    #region Update Display Calls

    public void UpdateDisplay(Displays p_id, int p_operation, bool p_value)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_value);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, float p_value = -99999, float p_data = -99999)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_value, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, int[] p_data)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_data);
    }

    public void UpdateDisplay(Displays p_id, int p_operation, object p_data)
    {
        displays[(int)p_id].UpdateDisplay(p_operation, p_data);
    }

    #endregion

    public object GetData(Displays p_id, int p_data)
    {
        return displays[(int)p_id].GetData(p_data);
    }

    private void OnActionRequested(Displays p_id, int p_action)
    {
        switch (p_id)
        {
            case Displays.INTRO:
                switch (p_action)
                {
                    case 0:
                        onPlayRequested?.Invoke();
                        break;
                    case 1:
                        ShowDisplay(Displays.CREDITS);
                        break;
                    case 2:
                        ShowDisplay(Displays.TUTORIAL);
                        break;
                }
                break;
            case Displays.CREDITS:
                switch (p_action)
                {
                    case 0:
                        ShowDisplay(Displays.INTRO);
                        break;
                    case 1:
                        Application.OpenURL("https://eliassanto13.wixsite.com/edevjogos");
                        break;
                }
                break;
            case Displays.GAME_OVER:
                switch (p_action)
                {
                    case 0:
                        onExitMatchRequested?.Invoke();
                        break;
                }
                break;
            case Displays.PAUSE:
                switch (p_action)
                {
                    case 1:
                        onUnpauseRequested?.Invoke();
                        break;
                }
                break;
        }
    }
}