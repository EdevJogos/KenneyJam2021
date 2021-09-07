using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager Instance;

    [System.Serializable]
    public class ClipData
    {
        #if UNITY_EDITOR_WIN
        public string name;
        #endif
        public SFXOccurrence occurrence;
        public float volume = 1;
        public AudioClip[] audioClip;
    }

    [System.Serializable]
    public class VoiceClipData
    {
        public Characters character;
        public ClipData[] voiceClips;
    }

    public AudioMixer audioMixer;
    public SFXSource sfxSourcePrefab;

    [Header("SFXS")]
    public GameObject sfxAudioSources;
    public AudioMixerGroup sfxMixerGroup;
    public ClipData[] sfxClips;
    public ClipData[] sfxUiClips;
    [Header("Voices")]
    public AudioMixerGroup voiceMixerGroup;
    public VoiceClipData[] voiceClips;

    public static bool Mute = false;
    public static SFXSource SFXSourcePrefab;
    public static Dictionary<SFXOccurrence, ClipData> SFXClips = new Dictionary<SFXOccurrence, ClipData>();
    public static Dictionary<Characters, ClipData[]> VoiceClips = new Dictionary<Characters, ClipData[]>();
    private static List<AudioSource> SFXSources = new List<AudioSource>(10);

    public void Initate()
    {
        Instance = this;
        SFXSourcePrefab = sfxSourcePrefab;
    }

    public void Initialize()
    {
        for (int __i = 0; __i < sfxClips.Length; __i++)
        {
            SFXClips.Add(sfxClips[__i].occurrence, sfxClips[__i]);
        }

        for (int __i = 0; __i < sfxUiClips.Length; __i++)
        {
            SFXClips.Add(sfxUiClips[__i].occurrence, sfxUiClips[__i]);
        }

        for (int __i = 0; __i < voiceClips.Length; __i++)
        {
            VoiceClips.Add(voiceClips[__i].character, voiceClips[__i].voiceClips);
        }

        for (int __i = 0; __i < 15; __i++)
        {
            AudioSource __source = sfxAudioSources.AddComponent<AudioSource>();

            SFXSources.Add(__source);
        }

        if(!PlayerPrefs.HasKey("DefaultVolumeSet"))
        {
            PlayerPrefs.SetFloat("MasterVol", 1f);
            PlayerPrefs.SetFloat("MusicVol", 1f);
            PlayerPrefs.SetFloat("SFXVol", 1f);
            PlayerPrefs.SetFloat("VoiceVol", 1f);

            PlayerPrefs.SetInt("DefaultVolumeSet", 1);
            PlayerPrefs.Save();
        }

        MasterVol(PlayerPrefs.GetFloat("MasterVol"));
        MusicVol(PlayerPrefs.GetFloat("MusicVol"));
        SFXVol(PlayerPrefs.GetFloat("SFXVol"));
        VoiceVol(PlayerPrefs.GetFloat("VoiceVol"));
    }

    public static void PlaySFX(SFXOccurrence p_occurrence, int p_index = 0, float p_pitch = 1, float p_pan = 0f)
    {
        for (int __i = 0; __i < SFXSources.Count; __i++)
        {
            if (!SFXSources[__i].isPlaying)
            {
                ClipData __data = SFXClips[p_occurrence];
                SFXSources[__i].outputAudioMixerGroup = Instance.sfxMixerGroup;
                PlayClip(SFXSources[__i], __data.audioClip[p_index], __data.volume, p_pitch, p_pan);

                break;
            }
        }
    }

    public static void PlaySFX(SFXOccurrence p_occurrence, AudioSource p_source, int p_index = 0, float p_pitch = 1, float p_pan = 0f)
    {
        ClipData __data = SFXClips[p_occurrence];
        p_source.outputAudioMixerGroup = Instance.sfxMixerGroup;
        PlayClip(p_source, __data.audioClip[p_index], __data.volume, p_pitch, p_pan);
    }

    public static void PlaySFX(Characters p_character, SFXOccurrence p_occurrence, AudioSource p_source, int p_index = 0, float p_pitch = 1, float p_pan = 0f)
    {
        ClipData[] __clips = VoiceClips[p_character];

        for (int __i = 0; __i < __clips.Length; __i++)
        {
            ClipData __data = __clips[__i];

            if(__data.occurrence == p_occurrence)
            {
                int __index = p_index < 0 ? Random.Range(0, __data.audioClip.Length + (p_index + 1)) : p_index;
                p_source.outputAudioMixerGroup = Instance.voiceMixerGroup;
                PlayClip(p_source, __data.audioClip[__index], __data.volume, p_pitch, p_pan);

                break;
            }
        }
    }

    public static void PlaySFX(SFXOccurrence p_occurrence, Vector2 p_position, int p_index = 0, float p_pitch = 1, float p_pan = 0f)
    {
        SFXSource __sfxSource = Instantiate(SFXSourcePrefab, p_position, Quaternion.identity);
        AudioSource __audioSource = __sfxSource.GetComponent<AudioSource>();
        ClipData __data = SFXClips[p_occurrence];

        __audioSource.outputAudioMixerGroup = Instance.sfxMixerGroup;
        __audioSource.clip = __data.audioClip[p_index];
        __audioSource.volume = __data.volume;
        __audioSource.Play();

        __sfxSource.enabled = true;
    }

    public static void InitalizeAudioSource(SFXOccurrence p_occurrence, AudioSource p_source, int p_index = 0)
    {
        ClipData __data = SFXClips[p_occurrence];

        p_source.clip = __data.audioClip[p_index];
        p_source.volume = __data.volume;
    }

    public static void StopSFXSmooth(AudioSource p_source, float p_time = 1f)
    {
        Instance.StartCoroutine(RoutineStopSFX(p_source, p_time));
    }

    private static void PlayClip(AudioSource p_source, AudioClip p_clip, float p_volume, float p_pitch = 1, float p_pan = 0f)
    {
        p_source.clip = p_clip;
        p_source.pitch = p_pitch;
        p_source.panStereo = p_pan;
        p_source.volume = p_volume;
        p_source.Play();
    }

    private static IEnumerator RoutineStopSFX(AudioSource p_source, float p_speed = 1f)
    {
        while (p_source.volume > 0f)
        {
            p_source.volume -= p_speed * Time.deltaTime;

            yield return null;
        }

        p_source.Stop();
    }

    public void MasterVol(float p_value)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(p_value) * 20f);
        PlayerPrefs.SetFloat("MasterVol", p_value);
    }

    public void MusicVol(float p_value)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(p_value) * 20f);
        PlayerPrefs.SetFloat("MusicVol", p_value);
    }

    public void SFXVol(float p_value)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(p_value) * 20f);
        PlayerPrefs.SetFloat("SFXVol", p_value);
    }

    public void VoiceVol(float p_value)
    {
        audioMixer.SetFloat("VoiceVol", Mathf.Log10(p_value) * 20f);
        PlayerPrefs.SetFloat("VoiceVol", p_value);
    }
}
