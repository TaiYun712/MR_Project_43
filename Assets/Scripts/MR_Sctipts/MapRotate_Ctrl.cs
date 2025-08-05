using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MapRotate_Ctrl : MonoBehaviour
{
    public Slider rotaSlider;
    public Transform mapRoot;

    public float rotateSpeed = 10f;
    
    public bool isDragging = false;
    
    void Start()
    {
        rotaSlider.value = 0.5f;
        
    }

    void Update()
    {
        
    }
}
