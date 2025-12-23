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
        
        Vector3 currentEulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentEulerAngles.y, 0f);
        
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }

    public void CallBackGuy_R()
    {
        transform.position = rightHandPos.position;
        
        Vector3 currentEulerAngles = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, currentEulerAngles.y, 0f);
        
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }
    
    
}
