using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumSetting : MonoBehaviour
{
    [SerializeField] AudioMixer gameMixer;
    [SerializeField] Slider bgmSlider, sfxSlider;


    void Start()
    {
        SetBGMVoleme();
        SetSFXVoleme();
    }

   public void SetBGMVoleme()
    {
        float vol = bgmSlider.value;
        gameMixer.SetFloat("bgmVolume", Mathf.Log10(vol)*20);
    }

    public void SetSFXVoleme()
    {
        float vol = sfxSlider.value;
        gameMixer.SetFloat("sfxVolume", Mathf.Log10(vol) * 20);

    }
}
