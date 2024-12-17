using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    public int Coins { get;  private set; }

    private List<bool> skinsAvailable;

    public int CurrSkinId { get; private set; }


    public List<string> infoAboutSkin { get; private set; }
    public List<int> pricesOfSkins { get; private set; }

    private void Awake()
    {
        skinsAvailable = new List<bool>();

        //initialize for test
        skinsAvailable.Add(true);
        skinsAvailable.Add(false);
        skinsAvailable.Add(false);
        Coins = 203;

        //info
        infoAboutSkin = new List<string>();
        infoAboutSkin.Add("Free.");
        infoAboutSkin.Add("Lock. \nBuy for 200 coins.");
        infoAboutSkin.Add("Lock. \nBuy for 500 coins.");
        //prices
        pricesOfSkins = new List<int>();
        pricesOfSkins.Add(0);
        pricesOfSkins.Add(200);
        pricesOfSkins.Add(500);

        CurrSkinId = 0;
        //
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinsPlusPlus(int sum)
    {
        Coins += sum;
    }

    public bool GetAvailablitySkin(int skinId)
    {
        return skinsAvailable[skinId];
    }

    public void SetCurrSkin(int skinId)
    {
        CurrSkinId = skinId;
    }

    public void BuySkin(int skinId)
    {
        Coins -= pricesOfSkins[skinId];
        skinsAvailable[skinId] = true;
    }
}
