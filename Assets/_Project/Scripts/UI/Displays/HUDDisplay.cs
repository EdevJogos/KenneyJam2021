using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDDisplay : Display
{
    public TMPro.TextMeshProUGUI speedText;
    public TMPro.TextMeshProUGUI lifeText;
    public TMPro.TextMeshProUGUI forceText;
    public TMPro.TextMeshProUGUI pointsText;

    public override void Initiate()
    {
        Player.onLifeUpdated += Player_onLifeUpdated;
        Player.onSpeedUpdated += Player_onSpeedUpdated;
        Barrier.onCharge += Barrier_onCharge;
        StageManager.onPointsUpdated += StageManager_onPointsUpdated;

        base.Initiate();
    }

    private void OnDestroy()
    {
        Player.onLifeUpdated -= Player_onLifeUpdated;
        Player.onSpeedUpdated -= Player_onSpeedUpdated;
        Barrier.onCharge -= Barrier_onCharge;
        StageManager.onPointsUpdated -= StageManager_onPointsUpdated;
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch(p_operation)
        {
            case 0:
                speedText.text = "Speed: " + p_value + "%";
                break;
            case 1:
                lifeText.text = "Life: " + p_value + "";
                break;
            case 2:
                forceText.color = p_value == 100 ? Color.green : Color.white;
                forceText.text = "Force: " + (p_value == 100 ? "Ready!" : (p_value + "%"));
                break;
            case 3:
                pointsText.text = "Points " + p_value + "";
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void StageManager_onPointsUpdated(int p_points)
    {
        UpdateDisplay(3, p_points, 0);
    }

    private void Barrier_onCharge(int p_force)
    {
        UpdateDisplay(2, p_force, 0);
    }

    private void Player_onSpeedUpdated(int p_speed)
    {
        UpdateDisplay(0, p_speed, 0);
    }

    private void Player_onLifeUpdated(int p_life)
    {
        UpdateDisplay(1, p_life, 0);
    }
}
