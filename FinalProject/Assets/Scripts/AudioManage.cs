using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManage : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string VolumePref = "VolumePref";
    private int firstPlayInt;
    public Slider VolumeSlider;
    private float VolumeValue;
    public AudioSource[] VolumeAudio;

    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
        if(firstPlayInt == 0)
        {
            VolumeValue = .25f;
            VolumeSlider.value = VolumeValue;
            PlayerPrefs.SetFloat(VolumePref, VolumeValue);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            VolumeValue = PlayerPrefs.GetFloat(VolumePref);
            VolumeSlider.value = VolumeValue;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(VolumePref, VolumeSlider.value);
    }

    void OnApplicationFocus(bool focus) //when pause the game or minimize the game, or exit the game
    {
        if (!focus)
        {
            SaveSoundSettings(); //Save the sound settings
        }
    }

    public void UpdateSound()
    {
        //VolumeAudio.volume = VolumeSlider.value;
        for(int i=0; i < VolumeAudio.Length; i++)
        {
            VolumeAudio[i].volume = VolumeSlider.value;
        }
    }
}
