using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCEO : MonoBehaviour
{
    private static GameCEO Instance;

    public static event System.Action onGameStateChanged;

    public static bool Tutorial { get; private set; } = true;
    public static GameState State { get; private set; }

    #if UNITY_EDITOR
    public GameState state;
    #endif

    public GUIManager guiManager;
    public InputManager inputManager;
    public CameraManager cameraManager;
    public AudioManager audioManager;
    public AgentsManager agentsManager;
    public StageManager stageManager;

    public ParticlesDabatase particlesDabatase;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Initiate();
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initiate()
    {
        Player.onGameOver += Player_onGameOver;

        inputManager.onPauseRequested += InputManager_onPauseRequested;

        guiManager.onPlayRequested += GuiManager_onPlayRequested;
        guiManager.onExitMatchRequested += GuiManager_onExitMatchRequested;
        guiManager.onUnpauseRequested += InputManager_onPauseRequested;

        guiManager.Initiate();
        audioManager.Initate();
        agentsManager.Initiate();
        stageManager.Initiate();
    }

    public void Initialize()
    {
        guiManager.Initialize();
        audioManager.Initialize();
        cameraManager.Initialize();

        guiManager.ShowDisplay(Displays.INTRO);
        ChangeGameState(GameState.INTRO);
    }

    //-----------------CEO------------------

    private void ChangeGameState(GameState p_state)
    {
        #if UNITY_EDITOR
            state = p_state;
        #endif

        State = p_state;
        onGameStateChanged?.Invoke();
    }

    private void Player_onGameOver()
    {
        ChangeGameState(GameState.GAME_OVER);
        guiManager.ShowDisplay(Displays.GAME_OVER);
    }

    private IEnumerator RoutineLoadScene(int p_scene, float p_delay = 0f)
    {
        if (p_delay > 0) yield return new WaitForSeconds(p_delay);

        var sceneLoader = SceneManager.LoadSceneAsync(p_scene);

        while (sceneLoader.progress <= 1)
        {
            yield return null;
        }
    }

    //-----------------INPUT MANAGER------------------

    private void InputManager_onPauseRequested()
    {
        if(State == GameState.PLAY)
        {
            Time.timeScale = 0f;
            ChangeGameState(GameState.PAUSE);
            guiManager.ShowDisplay(Displays.PAUSE);
        }
        else
        {
            Time.timeScale = 1f;
            ChangeGameState(GameState.PLAY);
            guiManager.ShowDisplay(Displays.HUD);
        }
    }

    //-----------------GUI MANAGER------------------

    private void GuiManager_onPlayRequested()
    {
        guiManager.ShowDisplay(Displays.HUD, 0, () => { Time.timeScale = 1f; ChangeGameState(GameState.PLAY); stageManager.StartStage(); });
    }

    private void GuiManager_onExitMatchRequested()
    {
        Time.timeScale = 1f;

        ChangeGameState(GameState.INTRO);

        stageManager.Restart();
        agentsManager.Restart();

        guiManager.ShowDisplay(Displays.INTRO);
    }

    //-----------------SCORE MANAGER----------------

    //-----------------STAGE MANAGER----------------

    //-----------------AGENTS MANAGER----------------
}
