using System.Collections;
using UnityEngine;

public class UIPoints : MonoBehaviour
{
    public TMPro.TextMeshProUGUI pointsText;

    public void SetPoints(int p_points)
    {
        pointsText.text = p_points + "";
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
