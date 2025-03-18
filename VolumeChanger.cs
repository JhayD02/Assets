using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Mirror;
using UnityEngine.UI;

public class VolumeChanger : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
    }

    public static void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

}