using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PowerUpDropper : MonoBehaviour
{
    public List<PowerUpDropChance> PowerUps;

    public void OnDisable()
    {
        List<PowerUpDropChance> dropped = new List<PowerUpDropChance>();
        foreach (PowerUpDropChance potentialDrop in PowerUps)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= potentialDrop.DropChance)
            {
                // Give a random weighted priority to certain drops
                potentialDrop.DropPriority = potentialDrop.PriorityOnDrop * UnityEngine.Random.Range(0.0f, 1.0f);
                dropped.Add(potentialDrop);
            }
        }
        if (dropped.Count > 0)
            CreatePowerUp(dropped.OrderByDescending(d => d.DropPriority).First());
    }

    private void CreatePowerUp(PowerUpDropChance pow)
    {
        Debug.Log("Dropped:" + pow.PowerUp.name);
        GameObject.Instantiate(pow.PowerUp.gameObject, transform.position + new Vector3(Grid.TileWidth*0.5f, 0.15f), Quaternion.identity);
    }
}

[System.Serializable]
public class PowerUpDropChance
{
    public PowerUp PowerUp;
    [Range(0.0f, 1.0f)]
    public float DropChance = 0.2f;
    [Range(1.0f, 10.0f)]
    public float PriorityOnDrop = 1;
    [HideInInspector]
    public float DropPriority = 0;
}
