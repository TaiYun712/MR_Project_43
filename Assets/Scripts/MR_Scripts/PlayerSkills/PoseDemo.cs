using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class PoseDemo : MonoBehaviour
{
    public bool isOpenOtherPanel;
    public bool isFalmFacing;

    [Header("目前使用技能")] 
    [SerializeField] //合成
    private bool isTableSkill = false;
    [SerializeField] //採集
    private bool isShootSkill = false;
    [SerializeField] //淨化
    private bool isCleanSkill = false;
    
    [Header("資訊面板")]
    public GameObject infoPanel_hand;
    public GameObject infoPanel_front;
    
    [Header("棲地合成台")]
    public GameObject tablePanel;
    public Animator tableAni;
    
    [Header("中指馬賽克")]
    public GameObject badHintPanel_L;
    public GameObject badHintPanel_R;

    [Header("技能面板")]
    public Text skillNameText;
    public GameObject skillPanel;
    public int currentSkill = 0;
    public int totalSkill = 4;

    [Header("發射能量球")]
    public GameObject ballPf;

    public GameObject handAim;
    public GameObject aimRay;
    public LineRenderer aimRayLine;
    public GameObject aimImpactPf;
    
    public float spawnSpeed = 5;
    public float ballDestoryTime = 5f; 
    public Transform shootPos;
    public float maxLineDistance = 5f;
    public LayerMask layerMask;

    public bool isShooting;
    
    [Header("清潔光束")]
    public bool isCleaning;

    public GameObject cleanRayHead;
    public GameObject cleanRay;
    public LineRenderer cleanRayLine;

    public GameObject cleanImpactPf;

    void Start()
    {
        isOpenOtherPanel = false;
        isFalmFacing = false;

        isShootSkill = true;
        
        infoPanel_hand.SetActive(false);
        infoPanel_front.SetActive(false);
        
        tablePanel.SetActive(false);
        tableAni.SetBool("opentable",false);
        
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);
        
        skillPanel.SetActive(false);
        
        aimRay.SetActive(false);
        aimImpactPf.SetActive(false);

        cleanRayHead.SetActive(false);
        cleanRay.SetActive(false);
        cleanImpactPf.SetActive(false);
        
        currentSkill = 0;

        isShooting = false;
        isCleaning = false;
        
    }

    private void Update()
    {
        //能量球發射瞄準線-右
        if (isShooting && isShootSkill)
        {
            HoldToAim();
        }
        else
        {
            CloseAimLine();
        }
        
        //淨化光束-右
        if (isCleaning && isCleanSkill) 
        {
            OpenCleanLaser();
        }
        else
        {
           CloseCleanLaser();
        }
            
            
        //偵測面板狀況-左
        if (!isOpenOtherPanel && IsPalmStillFacing() && !skillPanel.activeSelf)
        {
            ShowSkillPanel();
        }
        
    }

    //比中指bad bad-左、右
    public void OpenBadHint_R()
    {
        badHintPanel_R.SetActive(true);
    }

    public void OpenBadHint_L()
    {
        badHintPanel_L.SetActive(true);
        isOpenOtherPanel = true;
        
        HideSkillPanel();
    }

    public void CloseBadHint()
    {
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);

        isOpenOtherPanel = false;
    }
    
    //資訊面板-左
    public void OpenInfoPanel()  //手面板
    {
        infoPanel_hand.SetActive(true);
        
        isOpenOtherPanel = true;
        HideSkillPanel();
    }

    public void OpenFrontPanel()  //前方面板
    {
        infoPanel_front.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel_hand.SetActive(false);
        infoPanel_front.SetActive(false);
        
        isOpenOtherPanel = false;
    }

    //技能面板-左

    public void HandIsFacing()  //技能面板顯示
    {
        isFalmFacing = true;
        
        if (!isOpenOtherPanel)
        {
            ShowSkillPanel();
        }
    }

    public void HandFacingEnd()
    {
        isFalmFacing = false;
        HideSkillPanel();
    }

    bool IsPalmStillFacing()
    {
        return isFalmFacing;
    }
    
    public void ShowSkillPanel()
    {
        skillPanel.SetActive(true);
    }

    public void HideSkillPanel()
    {
        skillPanel.SetActive(false);
    }
    
    public void SwitchToNextSkill()   //技能面板切換
    {
        if (isFalmFacing)
        {
            currentSkill = (currentSkill + 1) % totalSkill;
            AudioManager.instance.SwitchSkillSound();

            switch (currentSkill)
            {
                case 0:
                    skillNameText.text = "能量球";
                    isShootSkill = true;
                    isTableSkill = false;
                    isCleanSkill = false;
                    
                    handAim.SetActive(true);
                    break;

                case 1:
                    skillNameText.text = "淨化";
                    isCleanSkill = true;
                    isShootSkill = false;
                    isTableSkill = false;
                    
                    handAim.SetActive(true);
                    break;

                case 2:
                    skillNameText.text = "棲地合成";
                    isTableSkill = true;
                    isShootSkill = false;
                    isCleanSkill = false;

                    handAim.SetActive(false);
                    break;

                case 3:
                    skillNameText.text = "移除";
                    isTableSkill = false;
                    isShootSkill = false;
                    isCleanSkill = false;
                    
                    handAim.SetActive(false);
                    break;
            }
        }
       
    }

    //棲地合成台-右
    public void OpenTablePanel()
    {
        if (isTableSkill)
        {
            tablePanel.SetActive(true);
            tableAni.SetBool("opentable",true);
        }
       
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

    //瞄準 & 辨認發射能量球或淨化光束-右
    public void HoldRock()
    {
        if (isShootSkill)
        {
            isShooting = true;
            AudioManager.instance.AimReadySound();
        }else if (isCleanSkill)
        {
            isCleaning = true;
            
        }
    }

    //採集用準心
    public void HoldToAim()
    {
        HoldTheRay(aimRay,aimRayLine,aimImpactPf);
    }

    public void ShootTheBall()
    {
        if (isShooting)
        {
            aimRay.SetActive(false);
            aimImpactPf.SetActive(false);
            AudioManager.instance.FireOutSound();
            
            GameObject shootBall =BulletPool.instance.GetBullet(shootPos.position, shootPos.rotation);
           
            Rigidbody shootBallRB = shootBall.GetComponent<Rigidbody>();
            shootBallRB.velocity = shootPos.forward * spawnSpeed;
          
            StartCoroutine(ReturnBulletAfterDelay(shootBall, ballDestoryTime));
           
            isShooting = false;
        }

        IEnumerator ReturnBulletAfterDelay(GameObject bullet,float delay)
        {
            yield return new WaitForSeconds(delay);

            if (bullet.activeInHierarchy)
            {
                BulletPool.instance.ReturnBullet(bullet);
            }
        }
        
    }
    
    //關閉瞄準線
    public void CloseAimLine()
    {
        aimRay.SetActive(false);
        aimImpactPf.SetActive(false);

        isShooting = false;
    }
    
    //淨化光束
    public void OpenCleanLaser()
    {
        if (isCleaning)
        {
            HoldTheRay(cleanRay,cleanRayLine,cleanImpactPf);
            
            cleanRayHead.SetActive(true);
            
        }
    }

    //關閉淨化光束
    public void CloseCleanLaser()
    {
        cleanRayHead.SetActive(false);
        cleanRay.SetActive(false);
        cleanImpactPf.SetActive(false);

        isCleaning = false;
    }

    //瞄準線
    void HoldTheRay(GameObject theRay,LineRenderer theRayLine,GameObject rayImpact)
    {
        Ray ray = new Ray(shootPos.position, shootPos.forward);
        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layerMask);
        
        theRay.SetActive(true);
        theRayLine.positionCount = 2;
        theRayLine.SetPosition(0,shootPos.position);
        
        Vector3 endPos = Vector3.zero;
        if (hasHit)
        {
            endPos = hit.point;
            rayImpact.SetActive(true);

            Quaternion aimImpactRotate = Quaternion.LookRotation(-hit.normal);
            rayImpact.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            rayImpact.transform.rotation = aimImpactRotate;
        }
        else
        {
            endPos = shootPos.position + shootPos.forward * maxLineDistance;
        }
        
        theRayLine.SetPosition(1,endPos);
    }

   

  



}
