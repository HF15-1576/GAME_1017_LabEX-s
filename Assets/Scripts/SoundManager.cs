using JetBrains.Annotations;
using UnityEngine;


public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
        Destroy (gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

}
