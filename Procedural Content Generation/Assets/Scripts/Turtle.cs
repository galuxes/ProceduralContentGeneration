using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public class Turtle : MonoBehaviour
{
    [SerializeField] private GridGeneration _gg;
    private List<List<Tile>> _grid;
    private Vector2Int _currLocation = Vector2Int.zero, _gridSize;
    private Tile _currTile;

    private Color _defaultColor;
    private void Start()
    {
        _gridSize = _gg.GetGridSize();
        _grid = _gg.GetGrid();
        _currTile = _grid[0][0];
        _defaultColor = _currTile.GetComponent<SpriteRenderer>().color;
        StartCoroutine(Wander());
    }
    
    

    private void Move(Direction dir)
    {
        _currTile.GetComponent<SpriteRenderer>().color = _defaultColor;
        switch (dir)
        {
            case Direction.Left:
                if ((_currLocation + Vector2Int.left).x > 0)
                {
                    _currLocation += Vector2Int.left;
                }
                _currTile.BreakWall(WallCode.Left);
                _currTile = _grid[_currLocation.x][_currLocation.y];
                _currTile.BreakWall(WallCode.Right);
                break;
            case Direction.Right:
                if ((_currLocation + Vector2Int.right).x < _gridSize.x - 1)
                {
                    _currLocation += Vector2Int.right;
                }
                _currTile.BreakWall(WallCode.Right);
                _currTile = _grid[_currLocation.x][_currLocation.y];
                _currTile.BreakWall(WallCode.Left);
                break;
            case Direction.Up:
                if ((_currLocation + Vector2Int.down).y > 0)
                {
                    _currLocation += Vector2Int.down;
                }
                _currTile.BreakWall(WallCode.Top);
                _currTile = _grid[_currLocation.x][_currLocation.y];
                _currTile.BreakWall(WallCode.Bottom);
                break;
            case Direction.Down:
                if ((_currLocation + Vector2Int.up).y < _gridSize.y - 1)
                {
                    _currLocation += Vector2Int.up;
                }
                _currTile.BreakWall(WallCode.Bottom);
                _currTile = _grid[_currLocation.x][_currLocation.y];
                _currTile.BreakWall(WallCode.Top);
                break;
        }
        _currTile.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            int ranNum = Random.Range(0, 4);
            Move((Direction)ranNum);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
