using UnityEngine;

public class Enemy : Character
{
    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        Debug.Log("Attacked for " + damage + " damage");
    }
}
