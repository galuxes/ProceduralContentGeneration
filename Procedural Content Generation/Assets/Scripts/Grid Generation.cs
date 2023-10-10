using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _tile;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2 _gridBuffer;
    //[SerializeField] private List<Tile> _grid = new List<Tile>();
    private List<List<Tile>> _grid = new List<List<Tile>>();

    private void Awake()
    {
        GenerateGrid();
    }

    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        DeleteGrid();
     
        Vector3 startPosition = transform.position, tilePosition;
     
        for (int y = 0; y < _gridSize.y; y++)
        {
            _grid.Add(new List<Tile>());
            for (int x = 0; x < _gridSize.x; x++)
            {
                tilePosition = Vector3.right * (x * _gridBuffer.x) + Vector3.down * (y * _gridBuffer.y);
                tilePosition += startPosition;
                var newObj = Instantiate(_tile, tilePosition, quaternion.identity, transform);
                var newTile = newObj.GetComponent<Tile>();
                _grid[y].Add(newTile);
                
                newTile.tileLocation = Vector2Int.right * x + Vector2Int.up * y;
            }
        }
    }
    
    [ContextMenu("Destroy Grid")]
    public void DeleteGrid()
    {
        if (_grid.Count > 0)
        {
            foreach (var list in _grid)
            {
                foreach (var tile in list.Where(tile => tile))
                {
                    DestroyImmediate(tile.gameObject);
                }

                list.Clear();
            }
            _grid.Clear();
        }
    }

    public List<List<Tile>> GetGrid()
    {
        return _grid;
    }

    public Vector2Int GetGridSize()
    {
        return _gridSize;
    }

}
