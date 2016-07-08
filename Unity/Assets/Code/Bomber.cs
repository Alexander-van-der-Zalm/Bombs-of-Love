using UnityEngine;
using System.Collections;

public class Bomber : MonoBehaviour
{
    //public GameObject BombGO;
    public Bomb bomb;
    public AudioClip bombDropFX;

    public int AvailableBombs = 2;
    public int MaxBombs = 2;

    public int BonusRange = 0;
    public int BonusDamage = 0;
    public float DetonateOverride = -1.0f;

    public bool OnBomb = false;

    public void ResetBombs()
    {
        AvailableBombs = MaxBombs;
    }

    public void DropBomb(Grid grid, Vector3 loc)
    {
        // Check if enough bombs are available
        if (AvailableBombs <= 0)
            return;

        // Check if it is a legal drop location
        Vector2 gridCoord = grid.GetGridCoordinates(loc);
        if (!grid.LegalBombLocation(gridCoord))
        {
            // Check if it can be place one above 
            // ####### Change this ########
            // to take into account top & bottom
            gridCoord += new Vector2(0, 1);
            if (!grid.LegalBombLocation(gridCoord))
                return;
        }

        // Check if there is already a bomb
        if (OnBomb)
            return;

        //Debug.Log("Drop bomb on loc: " + gridCoord);

        // Spawn a bomb on the grid middle location
        loc = grid.GetGridWorldPos(gridCoord, Grid.SnapSpotHor.Mid);
        GameObject droppedBomb = GameObject.Instantiate(bomb.gameObject, loc, Quaternion.identity) as GameObject;

        // Detonate bombs
        Bomb newBomb = droppedBomb.GetComponent<Bomb>();
        newBomb.Detonate(grid,this, gridCoord, BonusRange,BonusDamage);
        AudioSource.PlayClipAtPoint(bombDropFX, transform.position);

        // One less bomb available
        AvailableBombs--;
    }
}
