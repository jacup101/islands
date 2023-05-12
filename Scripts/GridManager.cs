using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;

    void Awake()
    {
        GenerateGrid(); 
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y=0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                // Creates a checkerboard grid (change true to isOffset)
                //var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(true);
            }
        }
    }
}
