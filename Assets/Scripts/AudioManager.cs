using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    
    
    [SerializeField]
    AudioSource bgmSource,uiSourse, animalSourse,sfxSource,sfxSource2;

    [Header("BGM")]
    [SerializeField]
    AudioClip titleBGM,gameBGM,gameWinBGM;

    public float titleBgmVol = 1f;
    public float gamingBgmVol = 0.6f;

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
        ,callTableSound,craftSuccessSound,craftFailSound,gameWinSFX,
        cleanStartSd,cleanKeepSd,cleanHitSd;
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
            SwitchTitleBGM();
        }
        else  if (sceneName == "Power_Destructible Mesh")
        {
            SwitchGameBGM();
        }
        else
        {
            bgmSource.Stop();
        }
        
    }

    public void SwitchTitleBGM()
    {
        bgmSource.clip = titleBGM;
        bgmSource.loop = true;
        bgmSource.volume = titleBgmVol;
        bgmSource.Play();
    }
    
    public void SwitchGameBGM()
    {
        bgmSource.clip = gameBGM;
        bgmSource.loop = true;
        bgmSource.volume = gamingBgmVol;
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
    
    //收集碎片
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

    //發射能量球
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

    //棲地合成
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
    
    //清潔光束
    public void OpenCleanBeam()
    {
        sfxSource.clip = cleanStartSd;
        sfxSource.Play();
    }

    public void KeepCleanBeam()
    {
        sfxSource2.clip = cleanKeepSd;
        sfxSource2.loop = true;
        sfxSource2.Play();
    }

    public void EndCleanBeam()
    {
        sfxSource2.Stop();
    }

    public void CleanHit()
    {
        sfxSource.clip = cleanHitSd;
        sfxSource.Play();
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
