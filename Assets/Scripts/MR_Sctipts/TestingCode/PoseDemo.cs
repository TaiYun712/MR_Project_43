using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseDemo : MonoBehaviour
{
    public GameObject infoPanel;
    public Text skillNameText;

    public int currentSkill = 0;
    public int totalSkill = 4;
    void Start()
    {
        infoPanel.SetActive(false);
        currentSkill = 0;
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
   
   
    
    
}
