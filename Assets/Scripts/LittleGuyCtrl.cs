using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Camera playerCam;
    
    public Transform leftHandPos;
    public Transform rightHandPos;

    public Transform outSidePos;
    
    public Animator guyAni;

    public Transform initialPos;
    public ParticleSystem shoeUpPrt;


    private void Awake()
    {
        transform.SetParent(initialPos);
    }

    //精靈坐在手上
    public void GuySitOnHand_L()
    {
        
        
        if(PoseDemo.isShootSkill || PoseDemo.isCleanSkill || PoseDemo.isTableSkill){return;}
        transform.position = leftHandPos.transform.position;
        guyAni.SetBool("gotosit",true);
        
        Vector3 lookDir = transform.position - playerCam.transform.position;
        if (lookDir.sqrMagnitude <= 0.0001f) { return; }
        transform.rotation = Quaternion.LookRotation(lookDir.normalized);
        
        transform.SetParent(leftHandPos);
    }

    public void GuySitOnHand_R()
    {
       
        
        if(PoseDemo.isShootSkill || PoseDemo.isCleanSkill || PoseDemo.isTableSkill){return;}
        transform.position = rightHandPos.transform.position;
        guyAni.SetBool("gotosit",true);
        
        Vector3 lookDir = transform.position - playerCam.transform.position;
        if (lookDir.sqrMagnitude <= 0.0001f) { return; }
        transform.rotation = Quaternion.LookRotation(lookDir.normalized);
        
        transform.SetParent(rightHandPos);
    }
    
    //精靈比讚
    public void GuySayGood()
    {
        guyAni.SetTrigger("isfine");
    }
    
    //抓起精靈說嗨
    public void HoldGuy()
    {
        guyAni.SetTrigger("sayhi");
    }

    public void LetGuyGoOutOfHand()
    {
        guyAni.SetBool("gotosit",false);
        transform.SetParent(outSidePos);
        
        Vector3 lookDir = transform.position - playerCam.transform.position;
        if (lookDir.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(lookDir.normalized);
    }
    
    //放下精靈 面對玩家
    public void LetGuyGo()
    {
        Vector3 lookDir = transform.position - playerCam.transform.position;
        if (lookDir.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        transform.rotation = Quaternion.LookRotation(lookDir.normalized);
    }

    public void CallBackGuy_L()
    {
       GuyShowUp(leftHandPos);
    }

    public void CallBackGuy_R()
    {
        GuyShowUp(rightHandPos);
    }

    public void GuyShowUp(Transform herePos)
    {
        transform.SetParent(outSidePos);
        transform.position = herePos.position;
        gameObject.SetActive(true);
        
        Vector3 currentEulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentEulerAngles.y, 0f);
        
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }
    
    
}
