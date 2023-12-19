using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    public Tile tilePrefab;
    private List<Tile> tiles = new List<Tile>();
    private int maxTileCount = 5;

    private float _zSpawn = 0;

    private float tileLenght = 121f;

    private Transform playerTransform;



    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnAndDestroyTile();
        SpawnAndDestroyTile();
        SpawnAndDestroyTile();
        SpawnAndDestroyTile();
    }

    void Update()
    {
        if (playerTransform.transform.position.z - 70 > _zSpawn - (tileLenght * 4))
        {
            SpawnAndDestroyTile();
        }
    }

    public void SpawnAndDestroyTile()
    {
        Tile newTile = Instantiate(tilePrefab);
        newTile.SetupTile(transform.forward * _zSpawn);
        tiles.Add(newTile);

        _zSpawn += tileLenght;

        // Eðer tiles listesi belirli bir sýnýrýn üzerine çýkarsa, en eski Tile'ý kaldýrabilirsiniz.
        if (tiles.Count > maxTileCount)
        {
            DestroyTile(tiles[0]);
        }
    }

    private void DestroyTile(Tile tile)
    {
        // Tile içindeki nesneleri temizleme veya baþka bir iþlem yapma
        Destroy(tile.gameObject);
        tiles.Remove(tile);
    }
}
