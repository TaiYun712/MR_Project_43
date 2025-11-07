using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Transform leftHandPos;
    public Transform rightHandPos;
    
    public Animator guyAni;

    public Transform initialPos;
    public ParticleSystem shoeUpPrt;
    
    void Start()
    {
        transform.position = initialPos.transform.position;

    }

    void Update()
    {
        
    }
    
    public void HoldGuy()
    {
        guyAni.SetTrigger("isfine");
    }

    public void CallBackGuy_L()
    {
        transform.position = leftHandPos.position;
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }

    public void CallBackGuy_R()
    {
        transform.position = rightHandPos.position;
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }
    
    
}
