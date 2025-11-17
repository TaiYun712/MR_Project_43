using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    
    
    [SerializeField]
    AudioSource bgmSource,uiSourse, animalSourse,sfxSource;

    [Header("BGM")]
    [SerializeField]
    AudioClip titleBGM,gameBGM,gameWinBGM;

    [Header("UI")]
    [SerializeField]
    AudioClip popSound,showHintSound,woodUISound,overHintSound,switchSound
        ,pickUpSound,putInSound;

    [Header("Animals")]
    [SerializeField]
    AudioClip happybirdSound,redbirdSound;

    [Header("SFX")]
    [SerializeField]
    AudioClip catchSoulSound,aimSound,fireSound,brokeSound,broadMoveSound,bubleUpSound
        ,callTableSound,craftSuccessSound,craftFailSound,gameWinSFX;
    [SerializeField]
    private AudioClip[] wallBrokenSounds;

   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //BGM
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "TitleScene")
        {
            bgmSource.clip = titleBGM;
            bgmSource.playOnAwake = true;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else  if (sceneName == "Power_Destructible Mesh")
        {
            bgmSource.clip = gameBGM;
            bgmSource.loop = true;
            bgmSource.Play();
        }
        else
        {
            bgmSource.Stop();
        }
        
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

    public void SettingBoardMove()
    {
        sfxSource.clip = broadMoveSound;
        sfxSource.Play();
    }

    public void UISound_Wood()
    {
        uiSourse.clip = woodUISound;
        uiSourse.Play();
    }

    public void BubleUp()
    {
        sfxSource.clip = bubleUpSound;
        sfxSource.Play();
    }
    
    //UI-Gaming
    public void ShowHint()
    {
        uiSourse.clip = showHintSound;
        uiSourse.Play();
    }

    public void SoulCatchOverHint()
    {
        uiSourse.clip = overHintSound;
        uiSourse.Play();
    }

    public void SwitchSkillSound()
    {
        uiSourse.clip = switchSound;
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

    public void WallBrokenSound()
    {
        int index = Random.Range(0, wallBrokenSounds.Length);
        uiSourse.clip = wallBrokenSounds[index];
        uiSourse.Play();
    }

    public void CallCraftTableSound()
    {
        sfxSource.clip = callTableSound;
        sfxSource.Play();
    }

    public void CraftSuccessSound()
    {
        sfxSource.clip = craftSuccessSound;
        sfxSource.Play();
    }

    public void CraftFailSound()
    {
        sfxSource.clip = craftFailSound;
        sfxSource.Play();
    }

    public void PickUpThePlantSound()
    {
        uiSourse.clip = pickUpSound;
        uiSourse.Play();
    }

    public void PutPlantInToCraft()
    {
        uiSourse.clip = putInSound;
        uiSourse.Play();
    }
    
    //BGM_GameOver
    public void PlayWinBGM()
    {
        sfxSource.clip = gameWinSFX;
        sfxSource.Play();
        
        bgmSource.clip = gameWinBGM;
        bgmSource.Play();
    }

  
}
