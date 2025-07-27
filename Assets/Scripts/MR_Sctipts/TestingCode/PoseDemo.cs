using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PoseDemo : MonoBehaviour
{
    public GameObject infoPanel_hand;
    public GameObject infoPanel_front;
    
    public GameObject tablePanel;
    public Animator tableAni;
    
    public GameObject badHintPanel_L;
    public GameObject badHintPanel_R;


    public Text skillNameText;

    public int currentSkill = 0;
    public int totalSkill = 4;

    public GameObject ballPf;
    
    public GameObject aimRay;
    public LineRenderer aimRayLine;
    
    public GameObject aimImpactPf;
    public float spawnSpeed = 5;
    public float ballDestoryTime = 5f; 
    public Transform shootPos;
    public float maxLineDistance = 5f;
    public LayerMask layerMask;

    public bool isShooting;
   

    void Start()
    {
        infoPanel_hand.SetActive(false);
        infoPanel_front.SetActive(false);
        
        tablePanel.SetActive(false);
        tableAni.SetBool("opentable",false);
        
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);
        
        aimRay.SetActive(false);

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
    public void OpenInfoPanel()  //手面板
    {
        infoPanel_hand.SetActive(true);
    }

    public void OpenFrontPanel()  //前方面板
    {
        infoPanel_front.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel_hand.SetActive(false);
        infoPanel_front.SetActive(false);
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

    //棲地合成台
    public void OpenTablePanel()
    {
        tablePanel.SetActive(true);
        tableAni.SetBool("opentable",true);
    }

    public void CloseTablePanel()
    {
        tableAni.SetBool("opentable",false);
        Invoke("WaitToClosePanel",0.2f);
    }

    public void WaitToClosePanel()
    {
        tablePanel.SetActive(false);
    }

    //發射能量球

    public void HoldRock()
    {
        isShooting = true;
        AudioManager.instance.AimReadySound();
    }

    public void HoldToAim()
    {
        Ray ray = new Ray(shootPos.position, shootPos.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);
        
        aimRay.SetActive(true);
        aimRayLine.positionCount = 2;
        aimRayLine.SetPosition(0,shootPos.position);
        
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
        
        aimRayLine.SetPosition(1,endPos);
    }

    public void ShootTheBall()
    {
        if (isShooting)
        {
            aimRay.SetActive(false);
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
