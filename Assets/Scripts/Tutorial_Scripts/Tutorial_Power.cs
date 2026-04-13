using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
public class Tutorial_Power : MonoBehaviour
{
   public bool isOpenOtherPanel;
    public bool isFalmFacing;
    public bool hasFirstGamePlay = false;

    [Header("目前使用技能")] 
    [SerializeField] //合成
    public static bool isTableSkill = false;
    [SerializeField] //採集
    public static bool isShootSkill = false;
    [SerializeField] //淨化
    public static bool isCleanSkill = false;
    
    [Header("資訊面板")]
    public GameObject infoPanel_hand;
    public GameObject infoPanel_front;
    
    [Header("中指馬賽克")]
    public GameObject badHintPanel_L;
    public GameObject badHintPanel_R;

    [Header("技能面板")]
    public Text skillNameText;
    public Text skillDescribeText;
    public GameObject skillPanel;
    public int currentSkill = 0;
    public int totalSkill = 4;

    public Image skillNameBg;
    public Image skillIntroBg;

    public Sprite sn_0,sn_1,sn_2,sn_3;
    public Sprite si_0, si_1, si_2, si_3;

    public GameObject l_HandHint;

    [Header("發射能量球")]
    public bool isShooting;
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
    
    [Header("能量球動畫")] 
    public Animator mainAimAni;
    public Animator frontAimAni;
    public Animator backAimAni;
    public GameObject energyPrt;

    public Animator cleanAimAni;
    
    [Header("清潔光束")]
    public bool isCleaning;

    public GameObject cleanImpactPf;
    public GameObject cleanHandAim;
    public GameObject cleanRayHead;
    public GameObject cleanRay;
    public LineRenderer cleanRayLine;
    
    public GameObject cleanTrigger;
    public float triggerSkin = 0.02f;
    public LayerMask dirtyLayerMask;
    
    [Header("棲地合成台")]
    public bool isOpenTable;
    public GameObject tablePanel;
    public Animator tableAni;

    [Header("中文文本顯示")] 
    public string skillName01_CN;
    public string skillName02_CN;
    public string skillName03_CN;
    public string skillName00_CN;
    [TextArea(2, 5)]
    public string skillIntro01_CN;
    [TextArea(2, 5)]
    public string skillIntro02_CN;
    [TextArea(2, 5)]
    public string skillIntro03_CN;
    [TextArea(2, 5)]
    public string skillIntro00_CN;
    
    [Header("英文文本顯示")]
    public string skillName01_EN;
    public string skillName02_EN;
    public string skillName03_EN;
    public string skillName00_EN;
    [TextArea(2, 5)]
    public string skillIntro01_EN;
    [TextArea(2, 5)]
    public string skillIntro02_EN;
    [TextArea(2, 5)]
    public string skillIntro03_EN;
    [TextArea(2, 5)]
    public string skillIntro00_EN;

    

    void Start()
    {
        SkillInitialState();
        hasFirstGamePlay = false;
        
    }

    private void Update()
    {
        if (TutorialManager.instance.currentState != TutorialManager.TutorialState.TutorialOpening)
        {
            if (!hasFirstGamePlay)
            {
                currentSkill =3;
                SetSkills();
                hasFirstGamePlay = true;
            }
            
            //偵測面板狀況-左
            if (!isOpenOtherPanel && IsPalmStillFacing() && !skillPanel.activeSelf)
            {
                ShowSkillPanel();
            }
        }
        else
        {
            SkillInitialState();
        }
            
    }

    private void FixedUpdate()
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
            cleanTrigger.SetActive(true);
        }
        else
        {
            CloseCleanLaser();
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

   
    #endregion

    #region 切換技能

    public void SwitchToNextSkill()   //技能面板切換
    {
        //能量球教學中
        if (TutorialManager.instance.currentState == TutorialManager.TutorialState.Skill_1_Tutorial &&
            currentSkill == 0)
        {
            return;
        }
        else if (isFalmFacing)
        {
            currentSkill = (currentSkill + 1) % totalSkill;
            AudioManager.instance.SwitchSkillSound();
            SetSkills();
        }
    }
    
    void CloseAllSkill()
    {
        isTableSkill = false;
        isShootSkill = false;
        isCleanSkill = false;
    }
    
    void SetSkills()
    {
        CloseAllSkill();
        
        PlantRecallManager.instance.RecallAllUnusePlants();
        
        switch (currentSkill)
        {
            case 0:
                skillNameBg.sprite = sn_1;
                skillIntroBg.sprite = si_1;
                l_HandHint.SetActive(false);
                
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    skillNameText.text = skillName01_CN;
                    skillDescribeText.text = skillIntro01_CN;
                }
                else
                {
                    skillNameText.text = skillName01_EN;
                    skillDescribeText.text = skillIntro01_EN;
                }
                
                CloseAllSkill();
                isShootSkill = true;
                    
                handAim.SetActive(true);
                energyPrt.SetActive(false);  
                cleanHandAim.SetActive(false);
                tablePanel.SetActive(false);
                PanelUI_Ctrl.instance.ClosePlantPeckUI();
                
                break;

            case 1:
                skillNameBg.sprite = sn_2;
                skillIntroBg.sprite = si_2;
                l_HandHint.SetActive(false);
                
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    skillNameText.text = skillName02_CN;
                    skillDescribeText.text = skillIntro02_CN;
                }
                else
                {
                    skillNameText.text = skillName02_EN;
                    skillDescribeText.text = skillIntro02_EN;
                }
                
                CloseAllSkill();
                isCleanSkill = true;
                
                
                cleanHandAim.SetActive(true);    
                handAim.SetActive(false);
                tablePanel.SetActive(false);
                PanelUI_Ctrl.instance.ClosePlantPeckUI();
                
                break;

            case 2:
                skillNameBg.sprite = sn_3;
                skillIntroBg.sprite = si_3;
                l_HandHint.SetActive(false);
                
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    skillNameText.text = skillName03_CN;
                    skillDescribeText.text = skillIntro03_CN;
                }
                else
                {
                    skillNameText.text = skillName03_EN;
                    skillDescribeText.text = skillIntro03_EN;
                }
                CloseAllSkill();
                isTableSkill = true;
                
                cleanHandAim.SetActive(false);
                handAim.SetActive(false);
                PanelUI_Ctrl.instance.OpenPlantPeckUI();
                
                break;

            case 3:
                skillNameBg.sprite = sn_0;
                skillIntroBg.sprite = si_0;
                l_HandHint.SetActive(true);
                
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    skillNameText.text = skillName00_CN;
                    skillDescribeText.text = skillIntro00_CN;
                }
                else
                {
                    skillNameText.text = skillName00_EN;
                    skillDescribeText.text = skillIntro00_EN;
                }
                CloseAllSkill();
                
                cleanHandAim.SetActive(false);
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
            isOpenTable = true;
            
            tablePanel.SetActive(true);
            tableAni.SetBool("opentable",true);
            AudioManager.instance.CallCraftTableSound();
        }
       
    }

    public void CloseTablePanel()
    {
        isOpenTable = false;
        
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
            energyPrt.SetActive(true); 
            AudioManager.instance.AimReadySound();
        }else if (isCleanSkill)
        {
            isCleaning = true;
            
            AudioManager.instance.OpenCleanBeam();
            AudioManager.instance.KeepCleanBeam();
            
        }
        else if(isTableSkill && isOpenTable)
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
        energyPrt.SetActive(false); 
        
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
            cleanAimAni.SetBool("iscleaning",true);
        }
    }

    //關閉淨化光束
    public void CloseCleanLaser()
    {
        AudioManager.instance.EndCleanBeam();
        cleanRayHead.SetActive(false);
        cleanRay.SetActive(false);
        cleanImpactPf.SetActive(false);
        cleanAimAni.SetBool("iscleaning",false);
        
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

            if (isCleaning && cleanTrigger != null)
            {
                Vector3 triggerPos = hit.point + hit.normal * triggerSkin;
                cleanTrigger.SetActive(true);
                cleanTrigger.transform.SetPositionAndRotation(triggerPos, aimImpactRotate);
            }
        }
        else
        {
            endPos = shootPos.position + shootPos.forward * maxLineDistance;
            
            if (cleanTrigger != null)
            {
                cleanTrigger.SetActive(false);
            }
        }
        
        theRayLine.SetPosition(1,endPos);
    }


    #endregion

    #region 技能初始設置

    void SkillInitialState()
    {
        isOpenOtherPanel = false;
        isFalmFacing = false;

        isShootSkill = false;
        
        infoPanel_hand.SetActive(false);
        infoPanel_front.SetActive(false);
        
        tablePanel.SetActive(false);
        tableAni.SetBool("opentable",false);
        isOpenTable = false;
        
        badHintPanel_L.SetActive(false);
        badHintPanel_R.SetActive(false);
        
        skillPanel.SetActive(false);
        
        handAim.SetActive(false);
        aimRay.SetActive(false);
        aimImpactPf.SetActive(false);

        cleanHandAim.SetActive(false);
        cleanRayHead.SetActive(false);
        cleanRay.SetActive(false);
        cleanImpactPf.SetActive(false);
        cleanTrigger.SetActive(false);
        
        currentSkill = 0;

        isShooting = false;
        isCleaning = false;
    }

    #endregion
}
