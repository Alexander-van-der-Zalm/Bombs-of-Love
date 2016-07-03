using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int Amount = 1;
    
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
    }
}
