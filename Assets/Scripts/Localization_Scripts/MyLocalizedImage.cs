using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MyLocalizedImage : MonoBehaviour
{
    public Image targetImage;

    [Header("中文 內容")] 
    public Sprite chineseSprite;

    [Header("英文 內容")]
    public Sprite englishSprite;
    
   
    void Start()
    {
        RefreshImage();
    }

    void RefreshImage()
    {
        if(targetImage == null){return;}

        if (LanguageManager.instance == null)
        {
            targetImage.sprite = chineseSprite;
            return;
        }

        if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
        {
            targetImage.sprite = chineseSprite;
        }
        else
        {
            targetImage.sprite = englishSprite;
        }
    }


}
