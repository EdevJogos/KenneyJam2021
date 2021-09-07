using UnityEngine;

public class ERotateZ : MonoBehaviour
{
    public float from, to;
    public float speed;

    private bool _going = true;
    private Vector3 _target;

    private Vector3 _v3Current, _v3From, _v3To;

    void Start()
    {
        _v3From = new Vector3(0, 0, from);
        _v3To = new Vector3(0, 0, to);
        _v3Current = _v3From;

        _target = _v3To;

        transform.eulerAngles = _v3Current;
    }

    void Update()
    {
        _v3Current = Vector3.MoveTowards(_v3Current, _target, Time.deltaTime * speed);

        if(Vector3.Distance(_v3Current, _target) < 0.1f)
        {
            _target = _going ? _v3From : _v3To;
            _going = !_going;
        }

        transform.eulerAngles = _v3Current;
    }
}