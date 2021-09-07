using UnityEngine;
using static TimerManager;
using static PrefabsDatabase;

public class AIShip : MonoBehaviour
{
    public static event System.Action onReflectDestroyed;
    public static event System.Action<AIShip> onDestroyed;

    public Colors colorID;
    public int hitPoints = 6;
    public Transform laserSpawnPoint;
    public SpriteRenderer spriteRenderer;

    private bool _exploding;
    private bool _changeDirection;
    private bool _canFire;
    private int _direction = 1;
    private float _directionSpeed;
    private Timer _directionTimer;
    private Timer _fireTimer;

    private void Start()
    {
        _directionTimer = AddTimer(new Timer(true, true, true, 3.5f, () => { _changeDirection = true; _directionSpeed = 0f; }));
        _fireTimer = AddTimer(new Timer(true, true, true, 3.0f, () => { _canFire = true; }));
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        if (_exploding)
            return;

        if(Vector2.Distance(transform.position, Vector2.zero) > 10.0f)
        {
            transform.position = Vector2.MoveTowards(transform.position, AgentsManager.PlayerPosition, 2.5f * Time.deltaTime);
            transform.right = (AgentsManager.PlayerPosition - (Vector2)transform.position);
        }
        else
        {
            transform.right = (AgentsManager.PlayerPosition - (Vector2)transform.position);

            if (_changeDirection)
            {
                if(_directionSpeed > 0)
                {
                    _directionSpeed = Mathf.MoveTowards(_directionSpeed, 0f, 5f * Time.deltaTime);
                }
                else
                {
                    _direction *= -1;
                    _changeDirection = false;
                }
            }
            else
            {
                _directionSpeed = Mathf.MoveTowards(_directionSpeed, 10f, 10f * Time.deltaTime);
                transform.RotateAround(Vector2.zero, Vector3.forward * _direction, _directionSpeed * Time.deltaTime);
            }
        }

        if (!CameraManager.InsideCamera(transform.position))
            return;

        if(_canFire)
        {
            Laser __laser = InstantiatePrefab<Laser>(Prefabs.LASER, 0, laserSpawnPoint.position, transform.rotation);
            AudioManager.PlaySFX(SFXOccurrence.LASER_SHOOT, 0, 1.2f, HelpExtensions.GetStereoPan(transform.position.x));
            __laser.Initialize(false, transform.right, 15f);
            __laser.ChangeColor(colorID);

            _canFire = false;
        }
    }

    public void Initialize(Colors p_colorID)
    {
        ChangeColor(p_colorID);
    }

    public void ChangeColor(Colors p_colorID)
    {
        colorID = p_colorID;
        spriteRenderer.color = StageManager.GetColor(p_colorID);
    }

    public void UpdateHitPoints(int p_amount, bool p_recover)
    {
        if (_exploding)
            return;

        hitPoints += p_amount;

        if(hitPoints <= 0)
        {
            if (p_recover) onReflectDestroyed?.Invoke();

            RequestDestroy();
        }
    }

    public void DestroyShip()
    {
        Destroy(gameObject);
    }

    public void RequestDestroy()
    {
        if (_exploding)
            return;

        _exploding = true;
        AudioManager.PlaySFX(SFXOccurrence.AI_EXPLOSION, 0, 1, HelpExtensions.GetStereoPan(transform.position.x));
        GetComponent<Animator>().SetTrigger("Explode");
        StageManager.GetScore(transform.position);
        CameraManager.ShakeScreen(0.1f, 0.1f);

        onDestroyed?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (_exploding)
            return;

        if(p_other.tag == "Laser")
        {
            Laser __laser = p_other.GetComponent<Laser>();

            if(__laser.reflected)
            {
                UpdateHitPoints(-6, true);
            }
            if(__laser.colorID == colorID)
            {
                UpdateHitPoints(-2, true);
            }
            else
            {
                UpdateHitPoints(-1, false);
            }

            __laser.RequestDestroy();
        }
        else if(p_other.tag == "ForceBarrier")
        {
            if (p_other.GetComponentInParent<ForceBarrier>().colorID == colorID)
            {
                UpdateHitPoints(-6, false);
            }
        }
        else if (p_other.tag == "Barrier")
        {
            if (p_other.GetComponentInParent<Barrier>().colorID == colorID)
            {
                UpdateHitPoints(-6, false);
            }
        }
    }
}
