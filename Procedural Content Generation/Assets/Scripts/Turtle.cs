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
    [SerializeField] private List<Direction> _visitableNeighboors = new List<Direction>();
    [SerializeField] private List<Tile> _queue;
    private Vector2Int _currLocation = Vector2Int.zero, _gridSize;
    private Tile _currTile;

    private Color _defaultColor;
    private void Start()
    {
        _gridSize = _gg.GetGridSize();
        _grid = _gg.GetGrid();
        _currTile = _grid[0][0];
        _queue.Add(_currTile);
        _currTile.visited = true;
        _defaultColor = _currTile.GetComponent<SpriteRenderer>().color;
        StartCoroutine(BFGrid());
    }

    private void UpdateNeighboors()
    {
        _visitableNeighboors.Clear();
        if ((_currLocation + Vector2Int.left).x >= 0 && !_grid[_currLocation.y][_currLocation.x-1].visited)
        {
            _visitableNeighboors.Add(Direction.Left);
        }

        if ((_currLocation + Vector2Int.right).x < _gridSize.x && !_grid[_currLocation.y][_currLocation.x+1].visited)
        {
            _visitableNeighboors.Add(Direction.Right);
        }

        if ((_currLocation + Vector2Int.down).y >= 0 && !_grid[_currLocation.y-1][_currLocation.x].visited)
        {
            _visitableNeighboors.Add(Direction.Up);
        }

        if ((_currLocation + Vector2Int.up).y < _gridSize.y && !_grid[_currLocation.y+1][_currLocation.x].visited)
        {
            _visitableNeighboors.Add(Direction.Down);
        }
    }

    private void Move(Direction dir)
    {
        _currTile.GetComponent<SpriteRenderer>().color = _defaultColor;
        switch (dir)
        {
            case Direction.Left:
                if ((_currLocation + Vector2Int.left).x >= 0)
                {
                    _currLocation += Vector2Int.left;
                    _currTile.BreakWall(WallCode.Left);
                    _currTile = _grid[_currLocation.y][_currLocation.x];
                    _currTile.BreakWall(WallCode.Right);
                }
                break;
            case Direction.Right:
                if ((_currLocation + Vector2Int.right).x < _gridSize.x)
                {
                    _currLocation += Vector2Int.right;
                    _currTile.BreakWall(WallCode.Right);
                    _currTile = _grid[_currLocation.y][_currLocation.x];
                    _currTile.BreakWall(WallCode.Left);
                }
                break;
            case Direction.Up:
                if ((_currLocation + Vector2Int.down).y >= 0)
                {
                    _currLocation += Vector2Int.down;
                    _currTile.BreakWall(WallCode.Top);
                    _currTile = _grid[_currLocation.y][_currLocation.x];
                    _currTile.BreakWall(WallCode.Bottom);
                }
                break;
            case Direction.Down:
                if ((_currLocation + Vector2Int.up).y < _gridSize.y)
                {
                    _currLocation += Vector2Int.up;
                    _currTile.BreakWall(WallCode.Bottom);
                    _currTile = _grid[_currLocation.y][_currLocation.x];
                    _currTile.BreakWall(WallCode.Top);
                }
                break;
        }
        _queue.Add(_currTile);
        _currTile.visited = true;
        _currTile.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            int ranNum = Random.Range(0, 4);
            Move((Direction)ranNum);
            yield return new WaitForEndOfFrame();
        }
    }
    
    private bool RecurseQueue()//return false if empty
    {
        while (_queue.Count > 0)
        {
            _currTile.GetComponent<SpriteRenderer>().color = _defaultColor;
            _currTile = _queue[0];
            _currLocation = _currTile.tileLocation;
            UpdateNeighboors();
            if (_visitableNeighboors.Count > 0)
            {
                _currTile.GetComponent<SpriteRenderer>().color = Color.green;
                return true;
            }
            else
            {
                _queue.Remove(_currTile);
            }
        }
        return false;
    }

    private bool RecurseStack()//return false if empty
    {
        while (_queue.Count > 0)
        {
            _currTile.GetComponent<SpriteRenderer>().color = _defaultColor;
            _currTile = _queue[^1];
            _currLocation = _currTile.tileLocation;
            UpdateNeighboors();
            if (_visitableNeighboors.Count > 0)
            {
                _currTile.GetComponent<SpriteRenderer>().color = Color.green;
                return true;
            }
            else
            {
                _queue.Remove(_currTile);
            }
        }
        return false;
    }
    
    private IEnumerator BFGrid()
    {
        bool running = true;
        while (running)
        {
            UpdateNeighboors();
            if (_visitableNeighboors.Count > 0)
            {
                int max = _visitableNeighboors.Count;
                int ranNum = Random.Range(0, max);
                //Debug.Log(ranNum);
                Move(_visitableNeighboors[ranNum]);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                running = RecurseQueue();
            }
        }
        Debug.Log("Done");
    }
}
