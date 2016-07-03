using UnityEngine;
using System.Collections;

public class Bomber : MonoBehaviour
{
    //public GameObject BombGO;
    public Bomb bomb;
    public int BonusRange = 0;
    public int BonusDamage = 0;

    public void SetBomb(Bomb newBomb)
    {
        bomb = newBomb;
        //BombGO = newBomb.gameObject;
    }

    public void DropBomb(Vector3 loc)
    {
        GameObject droppedBomb = GameObject.Instantiate(bomb.gameObject, loc, Quaternion.identity) as GameObject;
        Bomb newBomb = droppedBomb.GetComponent<Bomb>();
        newBomb.Detonate();
    }
}
