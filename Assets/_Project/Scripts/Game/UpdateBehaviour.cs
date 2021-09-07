using UnityEngine;

public class UpdateBehaviour : MonoBehaviour
{
    public GameState[] activeStates;

    public virtual void Awake()
    {
        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    public virtual void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    public virtual void OnGameStateChanged()
    {
        for (int __i = 0; __i < activeStates.Length; __i++)
        {
            if (activeStates[__i] == GameCEO.State)
            {
                enabled = true;
                return;
            }
        }

        enabled = false;
    }

    private void GameCEO_onGameStateChanged()
    {
        OnGameStateChanged();
    }
}
