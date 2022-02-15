using UnityEngine;
using static TimerManager;
using static PoolDatabase;

public class Player : Agent
{
    public static event System.Action onGameOver;
    public static event System.Action<int> onSpeedUpdated;
    public static event System.Action<int> onLifeUpdated;

    public Colors colorID;
    public int hitPoints = 10;
    public Laser laserPrefab;
    public Transform laserSpawnPoint;
    public Transform engine;

    private bool _exploding;
    private bool _canFire = true;
    private float _speed = 1f, _maxSpeed = 6f;
    public float acceleration = 3f;
    private Timer _canFireTimer;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        AIShip.onReflectDestroyed += AIShip_onReflectDestroyed;
    }

    private void Start()
    {
        _canFireTimer = AddTimer(new Timer(false, true, false, 0.2f, () => { _canFire = true; }));
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY && GameCEO.State != GameState.INTRO)
            return;

        if (_exploding)
            return;

        if(Input.GetMouseButton(0))
        {
            if(_canFire)
            {
                AudioManager.PlaySFX(SFXOccurrence.LASER_SHOOT);
                GetPooledObject<Laser>(Prefabs.LASER).Initialize(true, laserSpawnPoint.position, transform.rotation, transform.right, 40f);
                //Instantiate(laserPrefab, laserSpawnPoint.position, transform.rotation).Initialize(true, transform.right, 40f);
                _canFireTimer.Run();
                _canFire = false;
            }
        }

        if(Input.GetKey(KeyCode.W))
        {
            _speed = Mathf.MoveTowards(_speed, _maxSpeed, acceleration * Time.deltaTime);
            float __engineSize = (0.4f * _speed) / _maxSpeed;
            engine.localScale = new Vector3(__engineSize, __engineSize, __engineSize);
            onSpeedUpdated?.Invoke((int)((_speed * 100f) / _maxSpeed));
        }

        if(Input.GetKey(KeyCode.S))
        {
            _speed = Mathf.MoveTowards(_speed, 0f, acceleration * Time.deltaTime);
            float __engineSize = (0.4f * _speed) / _maxSpeed;
            engine.localScale = new Vector3(__engineSize, __engineSize, __engineSize);
            onSpeedUpdated?.Invoke((int)((_speed * 100f) / _maxSpeed));
        }

        transform.right = InputManager.MouseWorld - (Vector2)transform.position;

        if(Vector2.Distance(transform.position, InputManager.MouseWorld) > 2.5f)
        {
            _rigidbody2D.velocity = transform.right * _speed;
        }
        else
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    public void UpdateHitPoints(int p_amount)
    {
        if (_exploding)
            return;

        hitPoints += p_amount;

        onLifeUpdated?.Invoke(hitPoints);

        if (hitPoints <= 0)
        {
            RequestDestroy();
        }
    }

    public void DestroyShip()
    {
        onGameOver?.Invoke();
    }

    public void Restart()
    {
        _canFire = true;
        _exploding = false;
        _speed = 1f;
        hitPoints = 10;
        transform.position = Vector2.zero;

        float __engineSize = (0.4f * _speed) / _maxSpeed;
        engine.localScale = new Vector3(__engineSize, __engineSize, __engineSize);
        onSpeedUpdated?.Invoke((int)((_speed * 100f) / _maxSpeed));
        onLifeUpdated?.Invoke(hitPoints);
    }

    private void RequestDestroy()
    {
        _exploding = true;
        _rigidbody2D.velocity = Vector2.zero;
        AudioManager.PlaySFX(SFXOccurrence.PLAYER_EXPLOSION, 0, 1, HelpExtensions.GetStereoPan(transform.position.x));
        GetComponent<Animator>().SetTrigger("Explode");
    }

    private void AIShip_onReflectDestroyed()
    {
        UpdateHitPoints(1);
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag == "Laser")
        {
            Laser __laser = p_other.GetComponent<Laser>();

            CameraManager.ShakeScreen(0.5f, 0.5f);
            UpdateHitPoints(-1);

            __laser.RequestDestroy();
        }
        else if(p_other.tag == "AIShip")
        {
            AIShip __aiShip = p_other.GetComponentInParent<AIShip>();

            CameraManager.ShakeScreen(0.5f, 0.5f);
            UpdateHitPoints(-1);

            __aiShip.RequestDestroy();
        }
    }
}
