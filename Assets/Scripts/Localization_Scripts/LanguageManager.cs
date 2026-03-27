using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [Header("目前語言")] 
    public GameLanguage currentLanguage = GameLanguage.Chinese;
    
    public enum  GameLanguage
    {
        Chinese,
        English
    }

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

    public void SetLanguage(GameLanguage newLanguage)
    {
        currentLanguage = newLanguage;
    }
    
    public bool IsChinese()
    {
        return currentLanguage == GameLanguage.Chinese;
    }

    public bool IsEnglish()
    {
        return currentLanguage == GameLanguage.English;
    }

 
}
