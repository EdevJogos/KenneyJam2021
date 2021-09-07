using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Range VerticalLimit;
    public static Range HorizontalLimit;

    private static bool _ScreenShake;
    private static float _Strength;
    private static float _ScreenShakeTime;
    private static Vector3 _OriginalPosition;

    private Vector3 _randomVector;

    public static Camera MainCamera;
    public static Vector2 Center = new Vector2(-10f, 0f);

    public void Initialize()
    {
        MainCamera = Camera.main;
        _OriginalPosition = MainCamera.transform.localPosition;

        Vector2 __minLimits = MainCamera.ViewportToWorldPoint(new Vector2(0.0f, 0.0f));
        Vector2 __maxLimits = MainCamera.ViewportToWorldPoint(new Vector2(1.0f, 1.0f));

        VerticalLimit = new Range(__minLimits.y, __maxLimits.y);
        HorizontalLimit = new Range(__minLimits.x, __maxLimits.x);
    }

    private void Update()
    {
        if (_ScreenShake && MainCamera.enabled)
        {
            _randomVector = Random.insideUnitSphere;

            MainCamera.transform.localPosition = new Vector3(_randomVector.x * _Strength, _randomVector.y * _Strength, -10f) + _OriginalPosition;

            _ScreenShakeTime -= Time.deltaTime;

            if (_ScreenShakeTime <= 0)
            {
                StopShake();
            }
        }
    }

    public static void ShakeScreen(float p_duration, float p_strength)
    {
        _Strength = p_strength;
        _ScreenShakeTime += p_duration;
        _ScreenShakeTime = Mathf.Clamp(_ScreenShakeTime, 0f, p_duration);

        _ScreenShake = true;
    }

    public static void StopShake()
    {
        _ScreenShake = false;
        _ScreenShakeTime = 0f;

        MainCamera.transform.localPosition = _OriginalPosition;
    }

    public static bool InsideCamera(Vector2 p_position)
    {
        if (p_position.x < HorizontalLimit.min || p_position.x > HorizontalLimit.max)
            return false;

        if (p_position.y < VerticalLimit.min || p_position.y > VerticalLimit.max)
            return false;

        return true;
    }

    public static Vector2 RandomOutside()
    {
        float __x, __y = 0;

        if(Random.Range(0, 2) == 1)
        {
            __x = Random.Range(HorizontalLimit.min, HorizontalLimit.max);
            __y = VerticalLimit.max * 1.1f;
        }
        else
        {
            __x = Random.Range(0, 2) == 1 ? HorizontalLimit.min * 1.1f : HorizontalLimit.max * 1.1f;
            __y = Random.Range(VerticalLimit.min, VerticalLimit.max);
        }

        return new Vector2(__x, __y);
    }

    public static Vector2 RandomInside()
    {
        float __x, __y = 0;

        __x = Random.Range(HorizontalLimit.min, HorizontalLimit.max);
        __y = Random.Range(VerticalLimit.min, VerticalLimit.max);

        return new Vector2(__x, __y);
    }
}