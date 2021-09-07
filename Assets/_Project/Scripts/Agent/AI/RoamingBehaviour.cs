using UnityEngine;

public class RoamingBehaviour : MonoBehaviour
{
    TimerManager.Timer _directionTimer;

    public bool Moving { get { return _direction.magnitude > 0; } }
    public bool CanMove { get; set; }
    public float SpeedModf { get; set; } = 1;

    public float maxSpeed;
    public float acceleration;
    public Range directionTimer;

    private Vector2 _direction;
    private Vector2 _curSpeed;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //_directionTimer = TimerManager.AddTimer(new TimerManager.RandomTimer(true, true, directionTimer, ChooseDirection));
    }

    public void Roam()
    {
        if (!CanMove)
            return;

        float __targetX = _direction.x * maxSpeed * SpeedModf;
        float __targetY = _direction.y * maxSpeed * SpeedModf;

        _curSpeed.Set(Mathf.MoveTowards(_curSpeed.x, __targetX, acceleration),
                      Mathf.MoveTowards(_curSpeed.y, __targetY, acceleration));

        _rigidbody2D.MovePosition(_rigidbody2D.position + _curSpeed * Time.fixedDeltaTime);
    }

    public void Move()
    {
        CanMove = true;
    }

    public void Stop()
    {
        CanMove = false;
    }

    private void ChooseDirection()
    {
        int __x = 0, __y = 0;

        int __axis = Random.Range(0, 10);

        if (__axis >= 5) //Para cima ou para baixo
        {
            __y = Random.Range(0, 10) >= 5 ? 1 : -1;
        }

        __axis = Random.Range(0, 10);

        int __odds = __y == 0 ? 3 : 0; //Aumenta para 80% de chance o movimento horizontal caso y seja 0.

        if (__axis >= 5 - __odds) //Esquerda ou direita
        {
            __x = Random.Range(0, 10) >= 5 ? 1 : -1;
        }

        _direction.Set(__x, __y);
        _direction.Normalize();
    }
}
