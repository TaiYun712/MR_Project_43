
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RandomPoseGame : MonoBehaviour
{
    public Image randomPoseImage;

    public Sprite knife;
    public Sprite rock;
    public Sprite paper;

    public Sprite normal;

    public Animator gameStartAni;

    float countDownTime;
    public float poseOutTime = 2.0f;

    public int randomPoseIndex;
    

    bool isCountingDown = false;

    public Sprite winFace;
    public Sprite loseFace;
    public Sprite tieFace;
    public Sprite whatFace;


    void Start()
    {
        randomPoseImage.sprite = normal;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isCountingDown)
        {
            StartCoroutine(GameStartCoroutine());
        }
    }

    IEnumerator GameStartCoroutine()
    {
        Debug.Log("�}�l�q��");
        isCountingDown = true;

        gameStartAni.SetBool("gamestart", true);

        yield return new WaitForSeconds(poseOutTime);

        gameStartAni.SetBool("gamestart", false);

        randomPoseIndex = Random.Range(1, 4);

        switch (randomPoseIndex)
        {
            case 1:
                randomPoseImage.sprite = knife;

                if(Pose_Keyboard.isRock == true)
                {
                    Debug.Log("�AĹ�F!!");
                    Invoke("PlayerWin",1f);
                }else if(Pose_Keyboard.isPaper == true)
                {
                    Debug.Log("�A��F�C");
                    Invoke("PlayerLose", 1f);
                }
                else if(Pose_Keyboard.isKnife == true)
                {
                    Debug.Log("����~~");
                    Invoke("PlayerTie", 1f);
                }
                else
                {
                    Debug.Log("�X���� ����");
                    Invoke("PlayerDoNothing", 1f);
                }

                break;
            case 2:
                randomPoseImage.sprite = rock;

                if (Pose_Keyboard.isPaper == true)
                {
                    Debug.Log("�AĹ�F!!");
                    Invoke("PlayerWin", 1f);
                }
                else if (Pose_Keyboard.isKnife == true)
                {
                    Debug.Log("�A��F�C");
                    Invoke("PlayerLose", 1f);
                }
                else if (Pose_Keyboard.isRock == true)
                {
                    Debug.Log("����~~");
                    Invoke("PlayerTie", 1f);
                }
                else
                {
                    Debug.Log("�X���� ����");
                    Invoke("PlayerDoNothing", 1f);
                }
                break;
            case 3:
                randomPoseImage.sprite = paper;

                if (Pose_Keyboard.isKnife == true)
                {
                    Debug.Log("�AĹ�F!!");
                    Invoke("PlayerWin", 1f);
                }
                else if (Pose_Keyboard.isRock == true)
                {
                    Debug.Log("�A��F�C");
                    Invoke("PlayerLose", 1f);
                }
                else if (Pose_Keyboard.isPaper == true)
                {
                    Debug.Log("����~~");
                    Invoke("PlayerTie", 1f);
                }
                else
                {
                    Debug.Log("�X���� ����");
                    Invoke("PlayerDoNothing", 1f);
                }
                break;
        }

        Debug.Log("�X������");
        yield return new WaitForSeconds(3);
        randomPoseImage.sprite = normal;
        isCountingDown = false;
    }

    void PlayerWin()
    {
        randomPoseImage.sprite = loseFace;
    }

    void PlayerLose()
    {
        randomPoseImage.sprite = winFace;
    }

    void PlayerTie()
    {
        randomPoseImage.sprite = tieFace;
    }

    void PlayerDoNothing()
    {
        randomPoseImage.sprite = whatFace;
    }


    public void PoseToStart()
    {
        if (!isCountingDown)
        {
            StartCoroutine(GameStartCoroutine());
        }
    }
}
