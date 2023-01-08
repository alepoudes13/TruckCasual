using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    public KeyCode addScoreKey = KeyCode.N;
    public int addScoreValue = 10000;

    public KeyCode resetShopKey = KeyCode.M;

    ShopManager shopManager;
    // Start is called before the first frame update
    void Start()
    {
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(addScoreKey))
        {
            int totalScore = PlayerPrefs.GetInt("Score") + addScoreValue;
            PlayerPrefs.SetInt("Score", totalScore);
            shopManager.totalScoreText.text = "Total: " + totalScore;
        }
        if (Input.GetKeyDown(resetShopKey))
            shopManager.SetDefaultDatabase();
    }
}
