using UnityEngine;

public class GameOverDisplay : Display
{
    public TMPro.TextMeshProUGUI pointsText;

    public override void Initiate()
    {
        StageManager.onPointsUpdated += StageManager_onPointsUpdated;

        base.Initiate();
    }

    private void OnDestroy()
    {
        StageManager.onPointsUpdated -= StageManager_onPointsUpdated;
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch (p_operation)
        {
            case 0:
                pointsText.text = "Final Score: " + p_value;
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void StageManager_onPointsUpdated(int p_points)
    {
        UpdateDisplay(0, p_points, 0);
    }
}
