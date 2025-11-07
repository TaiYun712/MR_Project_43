using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Transform nearPlayerPos;
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

    public void CallBackGuy()
    {
        transform.position = nearPlayerPos.position;
        guyAni.SetTrigger("showup");
        shoeUpPrt.Play();
        AudioManager.instance.ShowHint();
    }
    
    
}
