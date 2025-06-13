using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseDemo : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject tablePanel;

    public Text skillNameText;

    public int currentSkill = 0;
    public int totalSkill = 4;

    public GameObject ballPf;
    public float spawnSpeed = 5;
    public Transform shootPos;

    public bool isShooting;
   

    void Start()
    {
        infoPanel.SetActive(false);
        tablePanel.SetActive(false);
        currentSkill = 0;

        isShooting = false;
        
    }

    //資訊面板
    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    //切換屬性
    public void SwitchToNextSkill()
    {
        currentSkill = (currentSkill + 1) % totalSkill;

        switch (currentSkill)
        {
            case 0:
                skillNameText.text = "無屬性";
                break;

            case 1:
                skillNameText.text = "淨化";

                break;

            case 2:
                skillNameText.text = "生長";

                break;

            case 3:
                skillNameText.text = "移除";

                break;
        }
    }

    //濕地合成台
    public void OpenTablePanel()
    {
        tablePanel.SetActive(true);
    }

    public void CloseTablePanel()
    {
        tablePanel.SetActive(false);

    }

    //發射能量球

    public void HoldRock()
    {
        isShooting = true;
    }

    public void ShootTheBall()
    {
        if (isShooting)
        {
            Vector3 bulletPos = shootPos.transform.position;
            
            GameObject shootBall = Instantiate(ballPf, bulletPos, Quaternion.identity);
            Rigidbody shootBallRB = shootBall.GetComponent<Rigidbody>();
            shootBallRB.velocity = transform.forward * spawnSpeed;
            isShooting = false;
        }
       
    }

  



}
