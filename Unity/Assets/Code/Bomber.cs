using UnityEngine;
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

        // Spawn a bomb on a grid location
        loc = grid.WorldToMidBottomSnappedGridPos(loc);
        GameObject droppedBomb = GameObject.Instantiate(bomb.gameObject, loc, Quaternion.identity) as GameObject;

        // Detonate bombs
        Bomb newBomb = droppedBomb.GetComponent<Bomb>();
        newBomb.Detonate(grid,this, BonusRange,BonusDamage);

        // One less bomb available
        AvailableBombs--;
    }
}
