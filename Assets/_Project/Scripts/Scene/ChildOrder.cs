using UnityEngine;

public class ChildOrder : MonoBehaviour
{
    public int order;
    public DynamicOrder dynamicOrder;

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = dynamicOrder.dynamicOrder + order;
    }
}
