using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuyCtrl : MonoBehaviour
{
    public Transform leftHandPos;
    public Transform rightHandPos;

    public Transform outSidePos;
    
    public Animator guyAni;

    public Transform initialPos;
    public ParticleSystem shoeUpPrt;

    public GameObject temDiolog;
    void Start()
    {
        transform.position = initialPos.transform.position;
        
        temDiolog.SetActive(false);
    }


    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("maphouse"))
        {
            temDiolog.SetActive(true);
            
            Invoke("CloseDiolog",2f);
        }
    }
*/
    void CloseDiolog()
    {
        temDiolog.SetActive(false);
    }


    public void HoldGuy()
    {
        guyAni.SetTrigger("isfine");
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
