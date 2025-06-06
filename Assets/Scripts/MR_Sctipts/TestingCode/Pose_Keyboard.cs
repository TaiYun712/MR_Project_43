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


    public Animator birdAni;

    void Start()
    {
        myPoseImage.sprite = knife;

        birdAni.SetBool("isfly",false);
    }

   //���� ����
   public void BirdFly()
    {
        birdAni.SetBool("isfly",true);
    }

    //========��ձ���

    public void PoseKnife()
    {
        myPoseImage.sprite = knife;
        isKnife = true;
        Debug.Log("���a�{�b�X  �ŤM");
    }

    public void PoseRock()
    {
        myPoseImage.sprite = rock;
        isRock = true;
        Debug.Log("���a�{�b�X  ���Y");
    }

    public void PosePaper()
    {
        myPoseImage.sprite = paper;
        isPaper = true;
        Debug.Log("���a�{�b�X  ��");
    }

    public void NoPose()
    {
        myPoseImage.sprite = normal;

        isKnife = false;
        isRock = false;
        isPaper = false;

        Debug.Log("���a�{���򳣨S�X");
    }
}
