using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    private Vector3[] spawnPos = new Vector3[] { new Vector3(-6.5f, 3, 170), new Vector3(0, 3, 170), new Vector3(6.5f, 3, 170) };

    float startDelay = 1;
    float repeateRate = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnObstacle", startDelay, repeateRate);
    }

    void spawnObstacle()
    {
        List<int> prefabsRange = GetRange();
        if (prefabsRange.Count == 0)
            prefabsRange.Add(0);
        int pos1 = Random.Range(0, 3), pos2 = Random.Range(0, 3);
        //if (playerControllerScript.gameOver == false)
            Instantiate(obstaclePrefabs[prefabsRange[Random.Range(0, prefabsRange.Count)]], spawnPos[pos1], obstaclePrefabs[0].transform.rotation);
        if(pos2 != pos1)
            Instantiate(obstaclePrefabs[prefabsRange[Random.Range(0, prefabsRange.Count)]], spawnPos[pos2], obstaclePrefabs[0].transform.rotation);
    }

    List<int> GetRange()
    {
        List<int> range = new List<int>();
        if (File.Exists(Application.persistentDataPath + "/ShopItems.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ShopItems.dat", FileMode.Open);

            for(int i = 0; i < obstaclePrefabs.Length; i++)
            {
                SaveData data = (SaveData)bf.Deserialize(file);
                if (data.state == 2)
                    range.Add(i);
            }
            file.Close();
        }
        return range;
    }
}