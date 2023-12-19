using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool _isBagder = false;
    private float _playerPosX;
    private void Start()
    {
        transform.position += Vector3.up * 3f;
        transform.Rotate(60 * Time.deltaTime, 0, 90);
    }
    void Update()
    {
        transform.Rotate(60 * Time.deltaTime, 0, 0);

        //badger algýlandýysa hareketi baþlat
        if (_isBagder)
        {
            AttractToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerManager.Instance.AddCoin(1);
            Destroy(gameObject);
        }

        if (other.tag == "Badger")
        {
            //Badger prefabýnda bulunan nesne ile çarpýþýrsa konuma doðru gider
            _playerPosX = PlayerManager.Instance.GetPlayerPositionX();
            _isBagder = true;
        }
    }

    private void AttractToPlayer()
    {
        // Hedef konumu belirle
        Vector3 targetPosition = new Vector3(_playerPosX, transform.position.y, transform.position.z);

        if (transform.position != targetPosition)
        {
            // Coinler player pozisyonuna çekilir
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 150 * Time.deltaTime);
        }
        else
        {
            // pozisyona gelen coin otomatik toplanýr
            PlayerManager.Instance.AddCoin(1);
            Destroy(gameObject);
        }

    }
}
