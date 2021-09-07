using UnityEngine;

public class YOrder : MonoBehaviour
{
    public int order;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 1000 - ((int)(transform.position.y * 100) + order);
    }
}
