using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private int baseHealth = 100; // Lebenspunkte der Basis

    public void TakeDamage(int damage)
    {
        baseHealth -= damage;

        if (baseHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Basis zerstört!");
        // Hier könnte das Spiel beendet oder ein Game Over ausgelöst werden
        Destroy(gameObject);
    }
}
