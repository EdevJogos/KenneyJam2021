using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event System.Action onPauseRequested;

    public static Vector2 MouseWorld { get { return CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition); } }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY && GameCEO.State != GameState.PAUSE)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseRequested?.Invoke();
        }
    }
}
