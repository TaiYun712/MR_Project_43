using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Transform nearPlayerPos;
    public Vector3 guyPos;

    public Animator guyAni;
    void Start()
    {
       Invoke("SetGuyPos",1f);
    }

    void Update()
    {
        
    }

    void SetGuyPos()
    {
        guyPos = new Vector3(nearPlayerPos.transform.position.x, nearPlayerPos.transform.position.y,
            nearPlayerPos.transform.position.z);
        gameObject.transform.position = guyPos;
    }

    public void HoldGuy()
    {
        guyAni.SetBool("isfine",true);
    }

    public void NotHoldGuy()
    {
        guyAni.SetBool("isfine",false);
        SetGuyPos();
    }
}
