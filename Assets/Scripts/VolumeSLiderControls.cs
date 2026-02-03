using UnityEngine;
using UnityEngine.UI;

public class VolumeSLiderControls : MonoBehaviour
{
    // Emun to hold correct values
    public enum VolumeType
    {
        Music,
        SFX
    }

    //Controls and stuff
    [SerializeField] private VolumeType volumeType;
    private Slider slider;

    private void Awake()
    {
      slider = GetComponent<Slider>();
    }

    private void Start()
    {
         slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
       if (volumeType == VolumeType.Music)
        {
            SoundManager.Instance.SetMusicVolume(value);
        }
        else if (volumeType == VolumeType.SFX)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
    }
}
