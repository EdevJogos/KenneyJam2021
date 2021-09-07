using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Laser : MonoBehaviour
{
    public bool belongPlayer;
    public bool reflected;
    public Colors colorID;
    public SpriteRenderer spriteRenderer;
    public Light2D light2D;

    private bool _destroying;
    private float _speed;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        GameCEO.onGameStateChanged += GameCEO_onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameCEO.onGameStateChanged -= GameCEO_onGameStateChanged;
    }

    private void GameCEO_onGameStateChanged()
    {
        if(GameCEO.State == GameState.GAME_OVER)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if (!CameraManager.InsideCamera(transform.position))
        {
            Destroy(gameObject);
        }

        if(_destroying)
        {
            _rigidbody2D.velocity = Vector2.MoveTowards(_rigidbody2D.velocity, Vector2.zero, 80f * Time.deltaTime);
        }
    }

    public void Initialize(bool p_player, Vector2 p_target, float p_speed)
    {
        belongPlayer = p_player;

        _speed = p_speed;
        _direction = p_target;
        _rigidbody2D.velocity = _direction * _speed;
    }

    public void Reverse()
    {
        PrefabsDatabase.InstantiatePrefab<ImpactLight>(Prefabs.BARRIER_IMPACT, 0, transform.position, transform.rotation).Initialize(spriteRenderer.color);

        reflected = true;
        _direction *= -1;
        _rigidbody2D.velocity = _direction * _speed * 2.0f;
    }

    public void DestroyLaser()
    {
        Destroy(gameObject);
    }

    public void RequestDestroy()
    {
        _destroying = true;
        AudioManager.PlaySFX(SFXOccurrence.LASER_EXPLOSION, 0, 1, HelpExtensions.GetStereoPan(transform.position.x));
        GetComponent<Animator>().SetTrigger("Explode");
        CameraManager.ShakeScreen(0.1f, 0.1f);
    }

    public void ChangeColor(Colors p_colorID)
    {
        colorID = p_colorID;
        spriteRenderer.color = StageManager.GetColor(p_colorID);
        //light2D.color = StageManager.GetColor(p_colorID);
    }
}
