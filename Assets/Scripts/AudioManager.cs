using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

   
    
    [SerializeField]
    AudioSource bgmSource,uiSourse, animalSourse,sfxSource;

    [Header("BGM")]
    [SerializeField]
    AudioClip titleBGM,gameBGM;

    [Header("UI")]
    [SerializeField]
    AudioClip popSound,showHintSound;

    [Header("Animals")]
    [SerializeField]
    AudioClip happybirdSound,redbirdSound;

    [Header("SFX")]
    [SerializeField]
    AudioClip catchSoulSound,aimSound,fireSound,brokeSound;

   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    //BGM
    void Start()
    {
        bgmSource.clip = titleBGM;
        bgmSource.playOnAwake = true;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void SwitchGameBGM()
    {
        bgmSource.clip = gameBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    
    //UI-TItle、Setting
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
    
    public void ShowHint()
    {
        uiSourse.clip = showHintSound;
        uiSourse.Play();
    }


    //SFX-Gaming
    public void CatchTheSoul()
    {
        sfxSource.clip = catchSoulSound;
        sfxSource.Play();
    }
    
    public void PlayRedbirdSound()
    {
        animalSourse.clip = redbirdSound;
        animalSourse.Play();
    }

    public void AimReadySound()
    {
        sfxSource.clip = aimSound;
        sfxSource.Play();
    }

    public void FireOutSound()
    {
        sfxSource.clip = fireSound;
        sfxSource.Play();
    }

    public void TargetBroken()
    {
        uiSourse.clip = brokeSound;
        uiSourse.Play();
    }

  
}
