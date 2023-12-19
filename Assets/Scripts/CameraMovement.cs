using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool _isStarted = false;
    private GameObject target;
    private Vector3 offset;
    Vector3 newPosition;

    private Animator animator;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    public void EndStartAnm()
    {
        animator.enabled = false;
        offset = transform.position - target.transform.position;
        _isStarted = true;
        PlayerManager.Instance.EndStartAnm();
    }

    void Update()
    {
        if (_isStarted)
        {
            newPosition = new Vector3(target.transform.position.x * 3 / 4, transform.position.y, offset.z + target.transform.position.z);
            transform.position = newPosition;
        }
    }
}
