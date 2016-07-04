﻿using UnityEngine;
using System.Collections;

public class Bomber : MonoBehaviour
{
    //public GameObject BombGO;
    public Bomb bomb;
    public int AvailableBombs = 2;
    public int MaxBombs = 2;

    public int BonusRange = 0;
    public int BonusDamage = 0;

    public void ResetBombs()
    {
        AvailableBombs = MaxBombs;
    }

    //public void SetBomb(Bomb newBomb)
    //{
    //    bomb = newBomb;
    //    //BombGO = newBomb.gameObject;
    //}

    public void DropBomb(Grid grid, Vector3 loc)
    {
        // Check if enough bombs are available
        if (AvailableBombs <= 0)
            return;

        // Check if it is a legal drop location
        Vector2 gridCoord = grid.GetGridCoordinates(loc);
        GridElement el = grid.GetGridElement(gridCoord);
        if (el.Type != GridElement.GridType.Floor)
        {
            // Check if it can be place one above 
            // ####### Change this ########
            // to take into account top & bottom
            gridCoord += new Vector2(0, 1);
            if (grid.GetGridElement(gridCoord).Type != GridElement.GridType.Floor)
                return;
        }

        // Check if there is already a bomb

        // Spawn a bomb on the grid middle location
        loc = grid.GetGridWorldPos(gridCoord, Grid.SnapSpot.Mid);
        GameObject droppedBomb = GameObject.Instantiate(bomb.gameObject, loc, Quaternion.identity) as GameObject;

        // Detonate bombs
        Bomb newBomb = droppedBomb.GetComponent<Bomb>();
        newBomb.Detonate(grid,this, BonusRange,BonusDamage);

        // One less bomb available
        AvailableBombs--;
    }
}
