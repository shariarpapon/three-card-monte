using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioElement[] sfxAudios;
    [SerializeField] private AudioElement[] musicAudios;

    private void Awake() 
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    public void PlaySFX(string id) 
    {
        foreach (AudioElement a in sfxAudios)
            if (a.id == id) sfxSource.PlayOneShot(a.clip);
    }

    public void SetMusic(string id) 
    {
        foreach (AudioElement a in musicAudios)
            if (a.id == id) 
            { 
                musicSource.clip = a.clip;
                musicSource.Play();
            }
    }

    public void UpdateVolumeLevels(float masterVol, float musicVol, float sfxVol) 
    {
        AudioListener.volume = masterVol;
        musicSource.volume = musicVol;
        sfxSource.volume = sfxVol;
    }

    public float GetMasterVolume() => AudioListener.volume;
    public float GetMusicVolume() => musicSource.volume;
    public float GetSFXVolume() => sfxSource.volume;

    public void SetMasterVolume(float value) { AudioListener.volume = value; }
    public void SetMusicVolume(float value) { musicSource.volume = value; }
    public void SetSFXVolume(float value) { sfxSource.volume = value;  }
}


#region Extra Utility

[System.Serializable]
public struct AudioElement 
{
    public string id;
    public AudioClip clip;
}

#endregion
