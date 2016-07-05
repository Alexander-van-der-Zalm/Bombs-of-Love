using UnityEngine;
using System.Collections;
using System;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpTypes
    {
        BombRange,
        MoreBombs
    }

    public PowerUpTypes Type;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Only players can pick up power ups
        if (other.GetComponent<Player>() == null)
            return;

        PowerUpPlayer(other.gameObject);

        CleanUp();
    }

    private void PowerUpPlayer(GameObject player)
    {
        Bomber bomber = player.GetComponent<Bomber>();
        switch(Type)
        {
            default:
            case PowerUpTypes.BombRange:
                bomber.BonusRange++;
                return;
            case PowerUpTypes.MoreBombs:
                bomber.AvailableBombs++;
                bomber.MaxBombs++;
                return;
        }
    }

    private void CleanUp()
    {
        // PickUpSound Trigger
        GameObject.Destroy(this.gameObject);
    }
}
