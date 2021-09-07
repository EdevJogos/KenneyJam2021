using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EInstensity : MonoBehaviour
{
    public bool playOnce;
    public float from, to, speed;

    private bool _going;
    private float _target;

    private void Start()
    {
        GetComponent<Light2D>().intensity = from;

        _target = to;
    }

    private void Update()
    {
        GetComponent<Light2D>().intensity = Mathf.MoveTowards(GetComponent<Light2D>().intensity, _target, speed * Time.deltaTime);

        if(Mathf.Abs(_target - GetComponent<Light2D>().intensity) <= 0)
        {
            if (playOnce) enabled = false;

            _target = _going ? from : to;
            _going = !_going;
        }
    }
}
