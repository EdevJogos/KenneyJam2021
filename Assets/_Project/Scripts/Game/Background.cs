using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public int totalStars;
    public GameObject starPrefab;
    public GameObject smallStarPrefab;

    private List<SpriteRenderer> _stars = new List<SpriteRenderer>();

    void Start()
    {
        for (int __i = 0; __i < totalStars; __i++)
        {
            GameObject __star = Instantiate(starPrefab, CameraManager.RandomInside(), Quaternion.identity);
            float __scale = Random.Range(0.1f, 0.6f);
            __star.transform.localScale = new Vector2(__scale, __scale);
            _stars.Add(__star.GetComponentInChildren<SpriteRenderer>());
        }

        for (int __i = 0; __i < 50; __i++)
        {
            GameObject __star = Instantiate(smallStarPrefab, CameraManager.RandomInside(), Quaternion.identity);
            float __scale = Random.Range(0.1f, 0.4f);
            __star.transform.localScale = new Vector2(__scale, __scale);
            _stars.Add(__star.GetComponentInChildren<SpriteRenderer>());
        }
    }

    public void UpdateStars(List<Colors> p_colors)
    {
        for (int __i = 0; __i < totalStars; __i++)
        {
            _stars[__i].color = StageManager.GetColor(p_colors[Random.Range(0, p_colors.Count)]);
            _stars[__i].SetAlpha(0.5f);
        }
    }
}
