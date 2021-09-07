using UnityEngine;
using UnityEngine.UI;

public class ShakeImage : MonoBehaviour
{
    private float _strength;
    private float _screenShakeTime;
    private Vector3? _originalPosition = null;

    private void Update()
    {
        Vector3 __randomVector = Random.insideUnitSphere;

        transform.localPosition = new Vector3(__randomVector.x * _strength, __randomVector.y * _strength, 0f) + _originalPosition.Value;

        _screenShakeTime -= Time.unscaledDeltaTime;

        if (_screenShakeTime <= 0)
        {
            StopShake();
        }
    }

    public void Shake(float p_duration, float p_strength, bool p_override = true)
    {
        if (!p_override && enabled)
            return;

        _strength = p_strength;
        _screenShakeTime += p_duration;
        _screenShakeTime = Mathf.Clamp(_screenShakeTime, 0f, p_duration);
        if(_originalPosition == null) _originalPosition = transform.localPosition;

        enabled = true;
    }

    public void StopShake()
    {
        enabled = false;
        _screenShakeTime = 0f;

        transform.localPosition = _originalPosition.Value;
    }
}
