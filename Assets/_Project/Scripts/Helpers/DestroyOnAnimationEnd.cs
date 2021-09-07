using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    private float _startTime;

    private void Start()
    {
        _startTime = Time.time;
    }

    private void Update()
    {
        float __duration = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        float __speed = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed;
        float __total = __duration * __speed;

        if( Time.time - _startTime > __total)
        {
            Destroy(gameObject);
        }
    }
}