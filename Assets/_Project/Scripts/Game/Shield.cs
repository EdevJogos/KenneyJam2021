using UnityEngine;

public class Shield : MonoBehaviour
{
    public float speed;
    public Transform target;

    private void Update()
    {
        transform.localPosition = target.localPosition;

        if(Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, Vector3.forward, speed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, -Vector3.forward, speed * Time.deltaTime);
        }
    }
}
