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

   //鳥動 控制
   public void BirdFly()
   {
     birdAni.SetBool("isfly",!birdAni.GetBool("isfly"));
   }

    //========手勢控制

    public void PoseKnife()
    {
        myPoseImage.sprite = knife;
        isKnife = true;
        Debug.Log("玩家現在出  剪刀");
    }

    public void PoseRock()
    {
        myPoseImage.sprite = rock;
        isRock = true;
        Debug.Log("玩家現在出  石頭");
    }

    public void PosePaper()
    {
        myPoseImage.sprite = paper;
        isPaper = true;
        Debug.Log("玩家現在出  布");
    }

    public void NoPose()
    {
        myPoseImage.sprite = normal;

        isKnife = false;
        isRock = false;
        isPaper = false;

        Debug.Log("玩家現什麼都沒出");
    }
}
