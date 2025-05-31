using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioSource bgmSource,uiSourse, animalSourse;

    [Header("BGM")]
    [SerializeField]
    AudioClip titleBGM;

    [Header("UI")]
    [SerializeField]
    AudioClip popSound;

    [Header("Animals")]
    [SerializeField]
    AudioClip happybirdSound;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        bgmSource.clip = titleBGM;
        bgmSource.playOnAwake = true;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void BublePopkeSound()
    {
        uiSourse.clip = popSound;
        uiSourse.Play();
    }

    public void SFXSoundTest()
    {
        animalSourse.clip = happybirdSound;
        animalSourse.Play();
    }


  
}
