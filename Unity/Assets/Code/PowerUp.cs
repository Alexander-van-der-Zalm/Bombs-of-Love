using UnityEngine;
using System.Collections;
using System;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpTypes
    {
        BombRange,
        MoreBombs,
        ExtraLife
    }

    public PowerUpTypes Type;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Only players can pick up power ups
        if (other.GetComponent<Player>() == null)
            return;

        PowerUpPlayer(other.gameObject);

        // Update UI

        CleanUp();
    }

    private void PowerUpPlayer(GameObject playerObj)
    {
        Bomber bomber = playerObj.GetComponent<Bomber>();
        Health health = playerObj.GetComponent<Health>();
        Player player = playerObj.GetComponent<Player>();
        switch (Type)
        {
            default:
            case PowerUpTypes.BombRange:
                bomber.BonusRange++;
                return;
            case PowerUpTypes.MoreBombs:
                bomber.AvailableBombs++;
                bomber.MaxBombs++;
                return;
            case PowerUpTypes.ExtraLife:
                player.Lives++;
                return;
        }
    }

    private void CleanUp()
    {
        // PickUpSound Trigger
        GameObject.Destroy(this.gameObject);
    }
}
