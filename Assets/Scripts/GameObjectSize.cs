using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 boxSize = GetComponent<Renderer>().bounds.size;
        string name = GetComponent<Transform>().name;
        Debug.Log("Name: "+ name + " = x: " + boxSize.x + " / y: " + boxSize.y + " / z: " + boxSize.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
