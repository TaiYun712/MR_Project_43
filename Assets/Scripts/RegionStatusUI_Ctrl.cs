using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionStatusUI_Ctrl : MonoBehaviour
{
    [Header("棲地狀況UI顯示")]
    public Text stateText;

    [Header("跟隨鏡頭")]
    public bool faceCamera = true;
    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if(!faceCamera){return;}
        
        if(targetCamera == null){targetCamera = Camera.main;}
        if(targetCamera == null){return;}
        
        Vector3 lookDir = transform.position - targetCamera.transform.position;
        if (lookDir.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(lookDir.normalized);
    }

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
