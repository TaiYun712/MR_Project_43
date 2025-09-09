using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Ctrl : MonoBehaviour
{
    public Plant plantData;
    
    void Start()
    {
        Debug.Log("這是個" + plantData.plantName);
    }

    public void PickUpThePlant()
    {
        Debug.Log("拿起" + plantData.plantName);
    }
    
    void Update()
    {
        
    }
}
