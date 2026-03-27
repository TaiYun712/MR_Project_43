using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;


public class LanguageSwitch : MonoBehaviour
{
    [Header("語言按鈕本身")]
    [SerializeField] 
    private Button languageButton;

    [Header("按鈕上顯示 EN / TW 的文字")]
    [SerializeField] 
    private Text languageButtonText;

    [Header("直接拖進來的語言 Locale")]
    [SerializeField] 
    private Locale chineseLocale;

    [SerializeField] 
    private Locale englishLocale;
    
    // 這個變數用來防止 XR / VR 按鈕一次觸發兩次
    private bool isSwitchingLanguage = false;
    
    // 確保 Addressables 和 Localization 都初始化完成
    private bool isLocalizationReady = false;
    
    public Text debugText; //測試文字

    private IEnumerator Start()
    {
        if (debugText != null)
        {
            debugText.text = "開始初始化...";
        }

        // 第一步：先手動初始化 Addressables
        AsyncOperationHandle<IResourceLocator> addressablesHandle = Addressables.InitializeAsync();
        yield return addressablesHandle;

        if (addressablesHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Addressables 初始化失敗");
            if (debugText != null)
            {
                debugText.text = "Addressables 初始化失敗";
            }

            yield break;
        }

        Debug.Log("Addressables 初始化成功");

        // 第二步：再等待 Localization 初始化完成
        yield return LocalizationSettings.InitializationOperation;

        isLocalizationReady = true;

        Debug.Log("Localization 初始化成功");

        if (debugText != null)
        {
            if (LocalizationSettings.SelectedLocale == null)
            {
                debugText.text = "Localization 初始化成功，但目前語言是 null";
            }
            else
            {
                debugText.text = "Localization 初始化成功，Locale = " + LocalizationSettings.SelectedLocale.Identifier.Code;
            }
        }

        // 初始化完成後更新按鈕文字
        UpdateButtonText();
    }
    
    
    // 這個方法要綁到 Button 的 OnClick
    public void ToggleLanguage()
    {
        // 還沒初始化完成就不允許切換
        if (isLocalizationReady == false)
        {
            Debug.LogWarning("Localization 尚未初始化完成");
            if (debugText != null)
            {
                debugText.text = "Localization 尚未初始化完成";
            }
            return;
        }
        ///////////////----------------------------------
        
        // 如果剛剛已經在切換，就直接忽略這次
        if (isSwitchingLanguage == true)
        {
            Debug.Log("這次語言切換被忽略");
            return;
        }

        // 如果按鈕元件沒指定，先擋掉
        if (languageButton == null)
        {
            Debug.LogWarning("languageButton 沒有指定");
            return;
        }

        // 如果中文或英文 Locale 沒指定，先擋掉
        if (chineseLocale == null || englishLocale == null)
        {
            Debug.LogWarning("中文或英文 Locale 沒有指定");
            return;
        }

        StartCoroutine(ToggleLanguageRoutine());
    }

    private IEnumerator ToggleLanguageRoutine()
    {
        isSwitchingLanguage = true;

        // 暫時把按鈕設成不能按，避免一次觸發兩次
        languageButton.interactable = false;

        Locale currentLocale = LocalizationSettings.SelectedLocale;

        // 如果目前沒有語言，就先切到中文
        if (currentLocale == null)
        {
            LocalizationSettings.SelectedLocale = chineseLocale;
            Debug.Log("目前沒有語言，改為中文");
        }
        else
        {
            // 判斷現在是哪個語言，再切到另一個
            if (currentLocale == chineseLocale)
            {
                LocalizationSettings.SelectedLocale = englishLocale;
                Debug.Log("已切換到英文");
            }
            else
            {
                LocalizationSettings.SelectedLocale = chineseLocale;
                Debug.Log("已切換到中文");
            }
        }

        // 更新按鈕上顯示的文字
        UpdateButtonText();
        AudioManager.instance.UISound_Wood();

        // 稍微等一下，避免 XR UI 同一次操作又觸發一次
        yield return new WaitForSeconds(0.25f);

        languageButton.interactable = true;
        isSwitchingLanguage = false;
    }

    private void UpdateButtonText()
    {
        if (languageButtonText == null)
        {
            return;
        }

        Locale currentLocale = LocalizationSettings.SelectedLocale;

        if (currentLocale == null)
        {
            languageButtonText.text = "TW";
            return;
        }

        // 如果現在是中文，按鈕顯示 EN，表示按下去會切到英文
        if (currentLocale == chineseLocale)
        {
            languageButtonText.text = "EN";
        }
        else
        {
            languageButtonText.text = "TW";
        }
    }
}
