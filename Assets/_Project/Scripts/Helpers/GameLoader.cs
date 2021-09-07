using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RoutineLoadGame());
    }

    private IEnumerator RoutineLoadGame()
    {
        var sceneLoader = SceneManager.LoadSceneAsync(1);

        while (sceneLoader.progress <= 1)
        {
            yield return null;
        }
    }
}
