using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum WallCode
{
    Top = 1000,
    Bottom = 0100,
    Left = 0010,
    Right = 0001
}

public enum TileState
{
    NoWalls = 0000,
    OnlyRight = 0001,
    OnlyLeft = 0010,
    LeftRight = 0011,
    OnlyBottom = 0100,
    BottomRight = 0101,
    BottomLeft = 0110,
    MissingTop = 0111,
    OnlyTop = 1000,
    TopRight = 1001,
    TopLeft = 1010,
    MissingBottom = 1011,
    TopBottom = 1100,
    MissingLeft = 1101,
    MissingRight = 1110,
    AllWalls = 1111,
}

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
    public TileState tileState = TileState.AllWalls;
    public Vector2Int tileLocation;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private SpriteRenderer _sr;
    
    
    public void BreakWall(WallCode wallCode)
    {
        int newTileState = (int)tileState - (int)wallCode;
        if (CheckIfBinary(newTileState))
        {
            tileState = (TileState)newTileState;
            UpdateSprite();
        }
    }

    [ContextMenu("Update Sprite")]
    private void UpdateSprite()
    {
        var index = Convert.ToInt32(((int)tileState).ToString(), 2);
        Debug.Log(index);
        _sr.sprite = _sprites[index];
    }
    
    private bool CheckIfBinary(int num)
    {
        while (num != 0) {
            if (num % 10 > 1) {
                return false;
            }
            num = num / 10;
        }
        return true;
    }
}
