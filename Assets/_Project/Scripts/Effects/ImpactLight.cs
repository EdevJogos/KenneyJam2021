using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ImpactLight : MonoBehaviour
{
    public Light2D light2D;

    public void Initialize(Color p_color)
    {
        light2D.color = p_color;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
