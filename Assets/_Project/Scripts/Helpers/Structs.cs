using UnityEngine;

[System.Serializable]
public struct Range
{
    /// <summary>
    /// Returns a random value between min and max.
    /// </summary>
    public float Value { get { return Random.Range(min, max); } }

    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
