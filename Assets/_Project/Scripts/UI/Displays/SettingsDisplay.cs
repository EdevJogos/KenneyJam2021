using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDisplay : Display
{
    public Slider masterSlider, musicSlider, sfxSlider, voiceSlider;

    public override void Show(bool p_show, Action p_callback, float p_ratio)
    {
        if(!p_show)
        {
            PlayerPrefs.Save();
        }

        base.Show(p_show, p_callback, p_ratio);
    }

    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch (p_operation)
        {
            case 0:
                UpdateVolumes();
                break;
        }

        base.UpdateDisplay(p_operation, p_value, p_data);
    }

    private void UpdateVolumes()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVol");
    }
}
