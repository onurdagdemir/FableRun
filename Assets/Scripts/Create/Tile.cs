using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    public GameObject[] groundPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject TunnelPrefab;
    public GameObject coinPrefab;
    public GameObject confettiPrefab;

    // Engellerin konumlarý
    List<float> obstaclePositions = new List<float> { -8f, 0f, 8f };

    List<GameObject> obstaclePrefabsList = new List<GameObject>();

    float tileLength = 121f;
    float obstacleGap = 70f;
    int obstaclePos = 0;
    int obstacleDensity = 40;
    int coinDensity = 40;

    bool isFence = false;
    int obstacleCounter = 0;
    int groundChoice = 0;
    int level = 0;
    int stage = 0;

    //Ýlgili aralýktaki obstacle' larý Instantiate yapmak için
    int obstaclePrefabsStart = 0;
    int obstaclePrefabsEnd = 15;

    public void SetupTile(Vector3 position)
    {
        //8 tile sonra level atla
        level = Convert.ToInt32((position.z / 121) / 8);
        ConfTile(level);
        // Tile'ý oluþtur
        transform.position = position;

        // Zemini oluþtur
        Instantiate(groundPrefabs[groundChoice], transform);

        // Obstacle'larý rastgele olarak oluþtur
        PlaceObstacles(position);
    }

    private void PlaceObstacles(Vector3 position)
    {
        for (float zPos = position.z + (obstacleGap / 2); zPos < position.z+tileLength-obstacleGap;)
        {
            isFence = false;
            obstacleCounter = 0;

            for (int i = 0; i < obstaclePositions.Count; i++) 
            {
                if(obstacleDensity > UnityEngine.Random.Range(0, 101))
                {
                    GameObject obstaclePrefab = obstaclePrefabs[UnityEngine.Random.Range(obstaclePrefabsStart, obstaclePrefabsEnd)];
                    if (obstaclePrefab.name.Contains("Fence"))
                    {
                        isFence = true;
                    }
                    obstaclePrefabsList.Add(obstaclePrefab);
                    obstacleCounter++;
                    if(obstacleCounter >= obstaclePositions.Count -1 && !isFence)
                    {
                        break;
                    }
                } else if(coinDensity > UnityEngine.Random.Range(0, 101))
                {
                    obstaclePrefabsList.Add(coinPrefab);
                    obstacleCounter++;
                    isFence = true;
                }
            }

            obstaclePos = UnityEngine.Random.Range(0, obstaclePositions.Count);

            foreach (GameObject obstaclePrefab in obstaclePrefabsList)
            {
                Instantiate(obstaclePrefab, new Vector3(obstaclePositions[obstaclePos % obstaclePositions.Count], 0f, zPos), Quaternion.Euler(0f, 180f, 0f), transform);
                obstaclePos++;
            }

            //engellerin arasýnda altýnlar oluþturur.
            zPos += obstacleGap / 4;

            Instantiate(coinPrefab, new Vector3(obstaclePositions[obstaclePos % obstaclePositions.Count], 0f, zPos), Quaternion.Euler(0f, 180f, 0f), transform);
            obstaclePos++;
            

            zPos += obstacleGap / 4;
            if(1 > UnityEngine.Random.Range(0, 2))
            {
                Instantiate(coinPrefab, new Vector3(obstaclePositions[obstaclePos % obstaclePositions.Count], 0f, zPos), Quaternion.Euler(0f, 180f, 0f), transform);
            }

            //engellerin arasýnda altýnlar oluþturur.
            zPos += obstacleGap / 4;

            //2 engel arasýnda tünel koyar random

            if (1 > UnityEngine.Random.Range(0, 10))
            {
                Instantiate(TunnelPrefab, new Vector3(0, 0, zPos), Quaternion.Euler(0f, 0f, 0f), transform);
                Instantiate(confettiPrefab, new Vector3(0, 0, zPos + 10f), Quaternion.Euler(0f, 0f, 0f), transform);
            }

            Instantiate(coinPrefab, new Vector3(obstaclePositions[obstaclePos % obstaclePositions.Count], 0f, zPos), Quaternion.Euler(0f, 180f, 0f), transform);
            obstaclePos++;

            zPos += obstacleGap / 4;
            obstaclePrefabsList.Clear();
        }


    }

    private void ConfTile(int level)
    {
        // harita seviyelerinin düzenlenmesi
        if(level <= 11)
        {
            stage = level;
        }
        else
        {
            stage = (level % 5) + 5; //bölümler tamamlandýðýnda 5. bölümden itibaren döngüye girsin.
        }
        switch (stage)
        {
            case 0:
            case 1:
                obstaclePrefabsStart = 0;
                obstaclePrefabsEnd = 8;
                groundChoice = 0;
                break;
            case 2:
                obstaclePrefabsStart = 8;
                obstaclePrefabsEnd = 15;
                groundChoice = 1;
                coinDensity = 60;
                obstacleGap = 60f;
                obstacleDensity = 55;
                break;
            case 3:
                obstaclePrefabsStart = 4;
                obstaclePrefabsEnd = 11;
                groundChoice = 2;
                coinDensity = 80;
                obstacleGap = 55f;
                obstacleDensity = 60;
                break;
            case 4:
                obstaclePrefabsStart = 0;
                obstaclePrefabsEnd = 15;
                groundChoice = 3;
                coinDensity = 90;
                obstacleGap = 50f;
                obstacleDensity = 70;
                break;
            case 5:
                obstaclePrefabsStart = 11;
                obstaclePrefabsEnd = 12;
                groundChoice = 0;
                obstacleGap = 52f;
                coinDensity = 90;
                obstacleDensity = 40;
                break;
            case 6:
                obstaclePrefabsStart = 0;
                obstaclePrefabsEnd = 15;
                groundChoice = 2;
                coinDensity = 90;
                obstacleGap = 50f;
                obstacleDensity = 70;
                break;
            case 7:
                obstaclePrefabsStart = 12;
                obstaclePrefabsEnd = 14;
                groundChoice = 1;
                coinDensity = 80;
                obstacleGap = 50f;
                obstacleDensity = 70;
                break;
            case 8:
                obstaclePrefabsStart = 8;
                obstaclePrefabsEnd = 9;
                groundChoice = 3;
                coinDensity = 90;
                obstacleGap = 52f;
                obstacleDensity = 40;
                break;
            case 9:
                obstaclePrefabsStart = 0;
                obstaclePrefabsEnd = 15;
                groundChoice = 4;
                coinDensity = 100;
                obstacleGap = 49f;
                obstacleDensity = 80;
                break;
            case 10:
                obstaclePrefabsStart = 0;
                obstaclePrefabsEnd = 15;
                groundChoice = 3;
                coinDensity = 100;
                obstacleGap = 49f;
                obstacleDensity = 90;
                break;
            case 11:
                obstaclePrefabsStart = 8;
                obstaclePrefabsEnd = 9;
                groundChoice = 2;
                coinDensity = 100;
                obstacleGap = 49f;
                obstacleDensity = 90;
                break;

        }
    }
}
