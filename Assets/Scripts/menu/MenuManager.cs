using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject customizePanel;
    [SerializeField] private GameObject dailyEventsPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject playPanel;

    [Header("Main panel")]
    [SerializeField] private Text textCoinsMain;

    [Header("Customize Panel")]
    [SerializeField] private List<GameObject> lockSkinsImages;
    [SerializeField] private List<GameObject> previewSkinImg;
    [SerializeField] private Text textInfo;
    [SerializeField] private Text textCoinsCustomize;

    [Header("Settings panel")]
    [SerializeField] private GameObject sfx_v;
    [SerializeField] private GameObject sfx_x;
    [SerializeField] private GameObject music_v;
    [SerializeField] private GameObject music_x;

    [Header("Daily panel")] 
    [SerializeField] private Text textDailyCoins;
    [SerializeField] private GameObject getDaily;
    [SerializeField] private GameObject vDaily;

    private int selectedCustSkin;

    private GeneralManager generalManager;

    // Start is called before the first frame update
    void Start()
    {
        generalManager = GetComponent<GeneralManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMusic()
    {
        music_x.SetActive(false);
        music_v.SetActive(true);
    }

    public void SetSFX()
    {
        sfx_x.SetActive(true);
        sfx_v.SetActive(false);
    }

    public void GetDaily()
    {
        textDailyCoins.text = "190";
        getDaily.SetActive(false);
        vDaily.SetActive(true);
        generalManager.CoinsPlusPlus(10);
    }

    #region customize

    private void SetLockForSkin(int indxSkin, bool isUnlock)
    {
        lockSkinsImages[indxSkin].SetActive(!isUnlock);
    }

    private void SetPreviewImg(int idSkin)
    {
        foreach (var skin in previewSkinImg)
        {
            skin.SetActive(false);
        }
        previewSkinImg[idSkin].SetActive(true);
    }

    private void SetInfoText(int idSkin)
    {
        textInfo.text = generalManager.GetAvailablitySkin(idSkin) ? "AVAILABLE" : generalManager.infoAboutSkin[idSkin];
    }

    private void SetMoneyText()
    {
        textCoinsCustomize.text = $"{generalManager.Coins}";
    }

    public void OnSelectPreview(int skinId)
    {
        selectedCustSkin = skinId;
        SetPreviewImg(skinId);
        SetInfoText(skinId);
    }

#endregion

#region openClose
    public void OpenPlayPanel()
    {
        mainMenuPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void ClosePlayPanel()
    {
        mainMenuPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void OpenCustomizePanel()
    {
        mainMenuPanel.SetActive(false);
        customizePanel.SetActive(true);

        for (int i = 0; i < 3; i++)
            SetLockForSkin(i, generalManager.GetAvailablitySkin(i));

        selectedCustSkin = generalManager.CurrSkinId;
        SetPreviewImg(selectedCustSkin);

        SetMoneyText();
    }
    public void CloseCustomizePanel()
    {
        if (generalManager.GetAvailablitySkin(selectedCustSkin))
        {
            generalManager.SetCurrSkin(selectedCustSkin);
            mainMenuPanel.SetActive(true);
            customizePanel.SetActive(false);
            RefrashMainPanel();
        } else
        {
            if (generalManager.Coins >= generalManager.pricesOfSkins[selectedCustSkin])
            {
                generalManager.BuySkin(selectedCustSkin);
                SetMoneyText();            
                SetLockForSkin(selectedCustSkin, generalManager.GetAvailablitySkin(selectedCustSkin));
                SetInfoText(selectedCustSkin);
            }
        }
    }

    public void OpenDailyEventsPanel()
    {
        mainMenuPanel.SetActive(false);
        dailyEventsPanel.SetActive(true);
    }
    public void CloseDailyEventsPanel()
    {
        mainMenuPanel.SetActive(true);
        dailyEventsPanel.SetActive(false);
        RefrashMainPanel();
    }

    public void OpenShopPanel()
    {
        mainMenuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void CloseShopPanel()
    {
        mainMenuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void RefrashMainPanel()
    {
        textCoinsMain.text = $"{generalManager.Coins}";
    }
#endregion

}
