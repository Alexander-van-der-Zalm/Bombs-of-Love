using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int MaxAmount = 1;
    public int Amount = 1;
    public bool IsDead = false;

   
    public void DoDamge(int damage)
    {
        Amount -= damage;
        Debug.Log("Damage:" + damage);
        if (Amount <= 0)
            Die();
    }

    public void Die()
    {
        Debug.Log("You Dead Son");
        IsDead = true;
    }

    public void Respawn()
    {
        IsDead = false;
        Amount = MaxAmount;
    }
}
