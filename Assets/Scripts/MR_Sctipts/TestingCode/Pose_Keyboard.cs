using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pose_Keyboard : MonoBehaviour
{
    public Image myPoseImage;

    public Sprite knife;
    public Sprite rock;
    public Sprite paper;

    public Sprite normal;

    public static bool isKnife = false;
    public static bool isRock = false;
    public static bool isPaper = false;


    void Start()
    {
        myPoseImage.sprite = knife;
    }

    /*
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            myPoseImage.sprite = knife;
            isKnife = true;
            Debug.Log("產瞷  芭");
        }
        else if (Input.GetKey(KeyCode.G))
        {
            myPoseImage.sprite = rock;
            isRock = true;
            Debug.Log("產瞷  ホ繷");
        }
        else if (Input.GetKey(KeyCode.H))
        {
            myPoseImage.sprite = paper;
            isPaper = true;
            Debug.Log("產瞷  ガ");
        }
        else
        {
            myPoseImage.sprite = normal;

            isKnife = false;
            isRock = false;
            isPaper = false;

            Debug.Log("產瞷ぐ或常⊿");
        }

    }
    */

    //========も墩北

    public void PoseKnife()
    {
        myPoseImage.sprite = knife;
        isKnife = true;
        Debug.Log("產瞷  芭");
    }

    public void PoseRock()
    {
        myPoseImage.sprite = rock;
        isRock = true;
        Debug.Log("產瞷  ホ繷");
    }

    public void PosePaper()
    {
        myPoseImage.sprite = paper;
        isPaper = true;
        Debug.Log("產瞷  ガ");
    }

    public void NoPose()
    {
        myPoseImage.sprite = normal;

        isKnife = false;
        isRock = false;
        isPaper = false;

        Debug.Log("產瞷ぐ或常⊿");
    }
}
