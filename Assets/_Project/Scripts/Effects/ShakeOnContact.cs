using UnityEngine;

public class ShakeOnContact : MonoBehaviour
{
    public string contactTag = "Feet";
    public float from, to;
    public float speed, duration;

    private bool _going = true, _shake = false;
    private float _timer;
    private Vector3 _target;

    private Vector3 _v3Current, _v3From, _v3To;

    void Start()
    {
        _v3From = new Vector3(0, 0, from);
        _v3To = new Vector3(0, 0, to);
        _v3Current = _v3From;

        _target = _v3To;
    }

    void Update()
    {
        if (!_shake)
            return;

        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            transform.eulerAngles = Vector3.zero;
            _shake = false;
        }
        else
        {
            _v3Current = Vector3.MoveTowards(_v3Current, _target, Time.deltaTime * speed);

            if (Vector3.Distance(_v3Current, _target) < 0.1f)
            {
                _target = _going ? _v3From : _v3To;
                _going = !_going;
            }

            transform.eulerAngles = _v3Current;
        }
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag == contactTag)
        {
            _shake = true;
            _timer = duration;
        }
    }
}
