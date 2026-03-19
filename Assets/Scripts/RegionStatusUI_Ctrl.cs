using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionStatusUI_Ctrl : MonoBehaviour
{
    [Header("棲地狀況UI顯示")]
    public Text stateText;

    public void SetText(string message)
    {
        if(stateText == null){return;}

        stateText.text = message;
    }

    public void ShowRegionStateUI()
    {
        gameObject.SetActive(true);
    }

    public void HideRegionStateUI()
    {
        gameObject.SetActive(false);
    }
}
