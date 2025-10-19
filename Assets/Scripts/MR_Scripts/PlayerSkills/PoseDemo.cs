using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public Text skillDescribeText;
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

    [Header("能量球動畫")] 
    public Animator mainAimAni;
    public Animator frontAimAni;
    public Animator backAimAni;
    
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

    #region 中指馬賽克

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

    #endregion

    #region 資訊面板
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
    

    #endregion

    #region 技能面板顯示

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

            SetSkills();
        }
    }
    #endregion

    #region 切換技能

    void SetSkills()
    {
        isShooting = false;
        isCleaning = false;
        isTableSkill = false;
        
        switch (currentSkill)
        {
            case 0:
                skillNameText.text = "能量球";
                skillDescribeText.text = "用以採集植物&泥土";
                isShootSkill = true;
                isTableSkill = false;
                isCleanSkill = false;
                    
                handAim.SetActive(true);
                tablePanel.SetActive(false);
                PanelUI_Ctrl.instance.ClosePlantPeckUI();
                break;

            case 1:
                skillNameText.text = "淨化";
                skillDescribeText.text = "清除環境中的髒污";
                isCleanSkill = true;
                isShootSkill = false;
                isTableSkill = false;
                    
                handAim.SetActive(true);
                tablePanel.SetActive(false);
                PanelUI_Ctrl.instance.ClosePlantPeckUI();
                break;

            case 2:
                skillNameText.text = "棲地合成";
                skillDescribeText.text = "用各種植物組成棲地";
                isTableSkill = true;
                isShootSkill = false;
                isCleanSkill = false;
                PlantRecallManager.instance.RecallAllUnusePlants();

                handAim.SetActive(false);
                PanelUI_Ctrl.instance.OpenPlantPeckUI();
                break;

            case 3:
                skillNameText.text = "移除";
                skillDescribeText.text = "移除過度生長的植物";
                isTableSkill = false;
                isShootSkill = false;
                isCleanSkill = false;
                    
                handAim.SetActive(false);
                tablePanel.SetActive(false);
                PanelUI_Ctrl.instance.ClosePlantPeckUI();
                break;
        }
    }

    #endregion

    #region 棲地合成台
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


    #endregion

    #region 握拳:瞄準or合成

    //瞄準 & 辨認發射能量球或淨化光束 & 觸發合成-右
    public void HoldRock()
    {
        // Debug.Log("拳頭被觸發！");
        if (isShootSkill)
        {
            isShooting = true;
            
            mainAimAni.SetTrigger("aiming");
            frontAimAni.SetBool("aiming",true);
            backAimAni.SetBool("aiming",true);
            AudioManager.instance.AimReadySound();
        }else if (isCleanSkill)
        {
            isCleaning = true;
            
        }
        else if(isTableSkill)
        {
            CraftingManager.instance.TryCraft();
            Debug.Log("觸發合成");
        }
        
    }

    #endregion
    
    #region 能量球&淨化光束
    
    //採集用準心
    public void HoldToAim()
    {
        HoldTheRay(aimRay,aimRayLine,aimImpactPf);
    }

    public void ShootTheBall()
    {
        if(!isShootSkill){return;}
        
       // Debug.Log("ShootTheBall 被觸發！");
        
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

        frontAimAni.SetBool("aiming",false);
        backAimAni.SetBool("aiming",false);
        
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


    #endregion
    
   
   

  



}
