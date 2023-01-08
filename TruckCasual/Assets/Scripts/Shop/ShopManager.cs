using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> prefabsList;
    List<ShopItem> itemsList = new List<ShopItem>();
    public GameObject purchButton;
    public GameObject canvas;
    public int distX = 50, distY = 50, startX = 50, startY = 50, itemRadius = 40, posZ = 40;

    public TextMeshProUGUI totalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        int totalScore = PlayerPrefs.GetInt("Score");
        totalScoreText.text = "Total: " + totalScore;

        itemsList = getItems();
        showItems(itemsList);
    }

    // Update is called once per frame
    public void UpdateScore(int price)
    {
        int totalScore = PlayerPrefs.GetInt("Score");
        totalScore -= price;
        totalScoreText.text = "Total: " + totalScore;
        PlayerPrefs.SetInt("Score", totalScore);
    }

    List<ShopItem> getItems()
    {

        List<ShopItem> itemsList = new List<ShopItem>();
        if (File.Exists(Application.persistentDataPath + "/ShopItems.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ShopItems.dat", FileMode.Open);
            foreach (GameObject prefab in prefabsList)
            {
                GameObject curPrefab = Instantiate(prefab), curButton = Instantiate(purchButton) as GameObject;
                curButton.transform.SetParent(canvas.transform, false);

                SaveData data = (SaveData)bf.Deserialize(file);
                ShopItem item = new ShopItem(curPrefab, curButton, data);
                itemsList.Add(item);
            }
            file.Close();
        }
        else
            SetDefaultDatabase();
        return itemsList;
    }

    void showItems(List<ShopItem> itemsList)
    {
        int x = startX, y = startY, ind = 1;

        foreach(ShopItem item in itemsList)
        {
            item.itemPrefab.transform.position = new Vector3(x, y, posZ);

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, item.itemPrefab.transform.position);
            item.purchaseButton.transform.localPosition = screenPoint - canvasRect.sizeDelta / 2f;
            x += distX;
            if(ind % 3 == 0)
            {
                x -= distX * 3;
                y += distY;
            }
            ind++;
        }
    }

    public void SaveItems()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/ShopItems.dat");
        
        foreach(ShopItem item in itemsList)
        {
            SaveData data = new SaveData();
            data.name = item.itemName;
            data.price = item.itemPrice;
            data.state = item.itemState;
            bf.Serialize(file, data);
        }
        file.Close();
    }

    public void SetDefaultDatabase()
    {
        itemsList.Clear();
        GameObject curPrefab = Instantiate(prefabsList[0]), curButton = Instantiate(purchButton) as GameObject;
        curButton.transform.SetParent(canvas.transform, false);
        SaveData dt = new SaveData();
        dt.name = "car";
        dt.price = 10000;
        dt.state = 2;
        ShopItem it = new ShopItem(curPrefab, curButton, dt);
        itemsList.Add(it);


        curPrefab = Instantiate(prefabsList[0]); curButton = Instantiate(purchButton) as GameObject;
        curButton.transform.SetParent(canvas.transform, false);
        dt = new SaveData();
        dt.name = "van";
        dt.price = 20000;
        dt.state = 0;
        it = new ShopItem(curPrefab, curButton, dt);
        itemsList.Add(it);

        curPrefab = Instantiate(prefabsList[0]); curButton = Instantiate(purchButton) as GameObject;
        curButton.transform.SetParent(canvas.transform, false);
        dt = new SaveData();
        dt.name = "bus";
        dt.price = 40000;
        dt.state = 0;
        it = new ShopItem(curPrefab, curButton, dt);
        itemsList.Add(it);

        curPrefab = Instantiate(prefabsList[0]); curButton = Instantiate(purchButton) as GameObject;
        curButton.transform.SetParent(canvas.transform, false);
        dt = new SaveData();
        dt.name = "tank";
        dt.price = 100000;
        dt.state = 0;
        it = new ShopItem(curPrefab, curButton, dt);
        itemsList.Add(it);

        SaveItems();
        itemsList.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

public class ShopItem
{
    public GameObject itemPrefab;
    public GameObject purchaseButton;
    public string itemName;
    public int itemPrice;
    public int itemState;

    public ShopItem(GameObject prefab, GameObject button, SaveData data)
    {
        itemPrefab = prefab;
        itemPrefab.GetComponent<Rigidbody>().useGravity = false;
        itemPrefab.GetComponent<OncomingMovement>().enabled = false;

        itemName = data.name;
        itemPrice = data.price;
        itemState = data.state;
        purchaseButton = button;
        purchaseButton.GetComponent<PurchaseButton>().SetText(TextOnState(itemState));
        purchaseButton.GetComponent<Button>().onClick.AddListener(ButtonClick);
        
    }

    void ButtonClick()
    {
        ShopManager shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        int totalScore = PlayerPrefs.GetInt("Score");
        if (itemState == 1) itemState = 2;
        else if (itemState == 0 && totalScore >= itemPrice)
        {
            itemState = 1;
            shopManager.UpdateScore(itemPrice);
        }
        else if (itemState == 2)
            itemState = 1;
        purchaseButton.GetComponent<PurchaseButton>().SetText(TextOnState(itemState));
        shopManager.SaveItems();
    }

    public string TextOnState(int state)
    {
        switch(state)
        {
            case 0:
                return "" + itemPrice;
            case 1:
                return "Purchased";
            case 2:
                return "Selected";
            default:
                return "Unknown";
        }
    }

}

[Serializable]
public class SaveData
{
    public string name;
    public int price;
    public int state;
}