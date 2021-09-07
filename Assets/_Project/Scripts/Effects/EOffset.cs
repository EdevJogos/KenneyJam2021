using UnityEngine;

public class EOffset : MonoBehaviour
{
    // Scroll main texture based on time

    public Vector2 scrollSpeed;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", offset);
    }
}
