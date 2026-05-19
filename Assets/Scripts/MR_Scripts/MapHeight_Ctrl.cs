using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeight_Ctrl : MonoBehaviour
{
    public Transform seePoint;
    
    void Start()
    {
        Invoke("SeyMapHeight",0.5f);
    }

    void SeyMapHeight()
    {
        Vector3 eyePos = seePoint.position;
        Vector3 targetPos = new Vector3(eyePos.x, eyePos.y-0.2f, eyePos.z + 0.5f);
        this.transform.position = new Vector3(targetPos.x+0.5f, targetPos.y-0.3f, targetPos.z+0.03f);
    }
   
    
}
