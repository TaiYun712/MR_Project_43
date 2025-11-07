using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Transform nearPlayerPos;
    public Animator guyAni;

    public Transform initialPos;
    
    void Start()
    {
        transform.position = initialPos.transform.position;

    }

    void Update()
    {
        
    }
    
    public void HoldGuy()
    {
        guyAni.SetBool("isfine",true);
    }

    public void CallBackGuy()
    {
        transform.position = nearPlayerPos.position;
    }
    
    
}
