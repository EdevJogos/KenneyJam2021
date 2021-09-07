using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public Colors RandomColor { get { return spawnColors[Random.Range(0, spawnColors.Count)]; } }

    public int totalShips;
    public List<Colors> spawnColors;
    public Range spawnDelay;
}