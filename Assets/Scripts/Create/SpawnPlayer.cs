using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject[] Chars;

    //Player Prefs karakter anahtarý
    private const string SelectionKey = "PlayerSelection";

    // Start is called before the first frame update

    private void Awake()
    {
        string savedSelection = PlayerPrefs.GetString(SelectionKey);
        foreach(GameObject c in Chars)
        {
            if(c.name == savedSelection)
            {
                Instantiate(c);
            }
        }
    }
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
