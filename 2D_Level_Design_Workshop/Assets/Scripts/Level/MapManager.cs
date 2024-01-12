using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private MapObject[] mapObjectsToSpawn;

    [SerializeField]
    private Tilemap objectTilemap;

    public static MapManager Instance;

    private void Start()
    {
        InitialiseMap();
    }

    public void InitialiseMap()
    {
        SpawnMapObjects(objectTilemap, mapObjectsToSpawn);
    }


    void SpawnMapObjects(Tilemap _map, MapObject[] _objects)
    {
        Debug.Log("Bounds: " + _map.cellBounds);
        TileBase t = null;
        Vector3Int pos = Vector3Int.zero;
        for (int i = 0; i < _objects.Length; i++)
        {
            for (int y = _map.cellBounds.min.y; y < _map.cellBounds.max.y; y++)
            {
                for (int x = _map.cellBounds.min.x; x < _map.cellBounds.max.x; x++)
                {
                    pos = new Vector3Int(x, y, 0);
                    t = _map.GetTile(pos);
                    if (t == null)
                        continue;

                    if (_objects[i].Tile == t)
                    {
                        // Spawn
                        SpawnMapObject(_objects[i].Object, pos);
                        DeleteSpawnedTileFromMap(_map, pos);
                    }
                }
            }
        }

    }

    void SpawnMapObject(GameObject _obj, Vector3 _pos)
    {
        Vector3 dif = new Vector3(0.5f, 0.5f);
        Debug.Log("SpawnMapObject!");
        Transform spawnedobj = Instantiate(_obj, _pos + dif, _obj.transform.rotation).transform;

        spawnedobj.parent = this.gameObject.transform;
    }

    void DeleteSpawnedTileFromMap(Tilemap _map, Vector3Int _pos)
    {
        _map.SetTile(_pos, null);
    }
}

[Serializable]
public struct MapObject
{
    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private TileBase tile;

    public TileBase Tile { get => tile; }
    public GameObject Object { get => obj; }
}
