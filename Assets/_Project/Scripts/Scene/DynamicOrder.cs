using UnityEngine;

public class DynamicOrder : MonoBehaviour
{
    public int order;
    public int dynamicOrder { get { return GetComponent<SpriteRenderer>().sortingOrder; } }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 1000 - ((int)(transform.position.y * 100) + order);
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 1000 - ((int)(transform.position.y * 100) + order);
    }
}
