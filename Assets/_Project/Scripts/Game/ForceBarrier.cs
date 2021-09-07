using UnityEngine;

public class ForceBarrier : MonoBehaviour
{
    public Colors colorID;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(3f, 2.5f), 10f * Time.deltaTime);

        if (!CameraManager.InsideCamera(transform.position))
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 p_direction)
    {
        _rigidbody2D.velocity = p_direction * 40f;
    }
}
