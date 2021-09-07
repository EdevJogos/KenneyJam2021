using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static event System.Action<int> onPointsUpdated;

    private static StageManager Instance;
    private static int _score;

    public AIShip aIShipPrefab;
    public Background background;
    public Color[] colors;
    public Stage[] stages;

    
    private int _stageIndex = 0;
    private List<AIShip> _activeShips = new List<AIShip>(20);

    public void Initiate()
    {
        Instance = this;

        AIShip.onDestroyed += AIShip_onDestroyed;
    }

    private void OnDestroy()
    {
        AIShip.onDestroyed -= AIShip_onDestroyed;
    }

    public void StartStage()
    {
        StartCoroutine(RoutineStage());
    }

    private IEnumerator RoutineStage()
    {
        for (int __j = 0; __j < stages.Length; __j++)
        {
            Stage __stage = stages[_stageIndex];
            background.UpdateStars(__stage.spawnColors);

            for (int __i = 0; __i < __stage.totalShips; __i++)
            {
                AIShip __aiShip = Instantiate(aIShipPrefab, CameraManager.RandomOutside(), Quaternion.identity);
                __aiShip.Initialize(__stage.RandomColor);
                _activeShips.Add(__aiShip);

                yield return new WaitForSeconds(Random.Range(__stage.spawnDelay.min, __stage.spawnDelay.max));
            }

            while(_activeShips.Count > 0)
            {
                yield return null;
            }

            _stageIndex++;
        }
    }

    public void Restart()
    {
        _stageIndex = 0;
        _score = 0;

        onPointsUpdated?.Invoke(_score);

        StopAllCoroutines();

        for (int __i = 0; __i < _activeShips.Count; __i++)
        {
            _activeShips[__i].RequestDestroy();
            __i--;
        }

        _activeShips.Clear();
    }

    private void AIShip_onDestroyed(AIShip p_aiShip)
    {
        _activeShips.Remove(p_aiShip);
    }

    public static Color GetColor(Colors p_colorID)
    {
        return Instance.colors[(int)p_colorID];
    }

    public static void GetScore(Vector2 p_position)
    {
        PrefabsDatabase.InstantiatePrefab<UIPoints>(Prefabs.POINTS, 0, p_position, Quaternion.identity).SetPoints(100);

        _score += 100;
        onPointsUpdated?.Invoke(_score);
    }
}
