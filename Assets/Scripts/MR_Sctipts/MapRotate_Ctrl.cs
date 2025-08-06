using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MapRotate_Ctrl : MonoBehaviour
{
    public Slider rotaSlider;
    public Transform mapRoot;

    public float rotateSpeed = 30f;
    
    public bool isDragging = false;
    
    void Start()
    {
        rotaSlider.value = 0.5f;
        
    }

    void Update()
    {
        if (isDragging)
        {
            float val = rotaSlider.value;
            float delta = val - 0.5f;

            if (Mathf.Abs(delta) > 0.01f)
            {
                float rotationAmount = -delta * rotateSpeed * Time.deltaTime;
                mapRoot.Rotate(Vector3.up,rotationAmount,Space.World);
            }
        }
    }

    //觸發滑桿時
    public void OnPointDown(BaseEventData eventData)
    {
        isDragging = true;
    }

    //未觸發滑桿時
    public void OnPointUp(BaseEventData eventData)
    {
        isDragging = false;

        if (Mathf.Abs(rotaSlider.value - 0.5f) > 0.01f)
        {
            rotaSlider.value = 0.5f;
        }
    }
}
