using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    private static readonly string VolumePref = "VolumePref";
    private float VolumeValue;
    public AudioSource[] VolumeAudio;
    void Awake()
    {
        ContinueSettings();
    }
    private void ContinueSettings()
    {
        VolumeValue = PlayerPrefs.GetFloat(VolumePref);
        //VolumeAudio.volume = VolumeValue;
        for (int i = 0; i < VolumeAudio.Length; i++)
        {
            VolumeAudio[i].volume = VolumeValue;
        }
    }
}
