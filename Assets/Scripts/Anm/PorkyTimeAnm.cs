using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorkyTimeAnm : MonoBehaviour
{
    private GameObject _timeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        _timeAnimation = transform.Find("Shine_blue").gameObject;
        _timeAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0.5f)
        {
            _timeAnimation.SetActive(true);
        }
        else
        {
            _timeAnimation.SetActive(false);
        }
    }
}
