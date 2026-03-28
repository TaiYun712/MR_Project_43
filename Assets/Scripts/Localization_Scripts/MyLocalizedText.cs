using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyLocalizedText : MonoBehaviour
{
    public Text targetText;

    [Header("中文 內容")] 
    [TextArea(2, 5)]
    public string chineseText;

    [Header("英文 內容")]
    [TextArea(2, 5)]
    public string englishText;
    void Start()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        if(targetText == null){return;}

        if (LanguageManager.instance == null)
        {
            targetText.text = chineseText;
            return;
        }

        if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
        {
            targetText.text = chineseText;
        }
        else
        {
            targetText.text = englishText;
        }
    }
    
  
}
