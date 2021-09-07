using UnityEngine;
using UnityEngine.UI;

public class PerfectCast : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }
}
