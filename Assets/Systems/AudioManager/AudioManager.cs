using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource audioSourceMusic;
    [SerializeField] AudioSource audioSourceUI;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlayAudioAtPosition(AudioClip clip,Vector3 Position)
    {

    }

    public void PlayUISound(AudioClip clip)
    {
        audioSourceUI.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSourceMusic.PlayOneShot(clip);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume));
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume));
    }
}
