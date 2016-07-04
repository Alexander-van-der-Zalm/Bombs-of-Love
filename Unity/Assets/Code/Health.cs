using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int MaxAmount = 1;
    public int Amount = 1;
    public bool IsDead = false;

    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    public void DoDamge(int damage)
    {
        Amount -= damage;
        Debug.Log("Damage:" + damage);
        if (Amount <= 0)
            Die();
    }

    public void Die()
    {
        IsDead = true;
        if (player != null)
            player.Death();
    }

    public void Respawn()
    {
        IsDead = false;
        Amount = MaxAmount;
    }
}
