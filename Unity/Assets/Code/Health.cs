using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int MaxAmount = 1;
    public int Amount = 1;
    public bool IsDead = false;
    public bool Invulnerable = false;

    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    // ########## if working with hp remember owner of flame ##############
    public void DoDamage(int damage)
    {
        if (Invulnerable || IsDead)
            return;

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
