using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseDemo : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject tablePanel;
    public GameObject badHintPanel_L;
    public GameObject badHintPanel_R;


    public Text skillNameText;

    public int currentSkill = 0;
    public int totalSkill = 4;

    public GameObject ballPf;
    public GameObject aimLine;
    public GameObject aimImpactPf;
    public float spawnSpeed = 5;
    public float ballDestoryTime = 5f; 
    public Transform shootPos;
    public float maxLineDistance = 5f;
    public LayerMask layerMask;

    public bool isShooting;
   

    void Start()
    {
        infoPanel.SetActive(false);
        tablePanel.SetActive(false);
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);
        
        aimLine.SetActive(false);
        currentSkill = 0;

        isShooting = false;
        aimImpactPf.SetActive(false);
    }

    private void Update()
    {
        if (isShooting)
        {
            HoldToAim();
        }
    }

    //比中指bad bad
    public void OpenBadHint_R()
    {
        badHintPanel_R.SetActive(true);
    }

    public void OpenBadHint_L()
    {
        badHintPanel_L.SetActive(true);
    }

    public void CloseBadHint()
    {
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);
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
        aimLine.SetActive(true);
        AudioManager.instance.AimReadySound();
    }

    public void HoldToAim()
    {
        Ray ray = new Ray(shootPos.position, shootPos.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);

        Vector3 endPos = Vector3.zero;
        if (hasHit)
        {
            endPos = hit.point;
            aimImpactPf.SetActive(true);

            Quaternion aimImpactRotate = Quaternion.LookRotation(-hit.normal);
            aimImpactPf.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            aimImpactPf.transform.rotation = aimImpactRotate;
        }
        else
        {
            endPos = shootPos.position + shootPos.forward * maxLineDistance;
        }
    }

    public void ShootTheBall()
    {
        if (isShooting)
        {
            aimLine.SetActive(false);
            aimImpactPf.SetActive(false);
            AudioManager.instance.FireOutSound();
            
            Vector3 bulletPos = shootPos.transform.position;
            
            GameObject shootBall = Instantiate(ballPf, bulletPos, Quaternion.identity);
            Rigidbody shootBallRB = shootBall.GetComponent<Rigidbody>();
            shootBallRB.velocity = shootPos.forward * spawnSpeed;
            Destroy(shootBall,ballDestoryTime);
            
            isShooting = false;
        }
       
    }

  



}
