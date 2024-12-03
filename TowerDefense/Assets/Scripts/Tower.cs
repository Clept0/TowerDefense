using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f; // Reichweite des Turms
    [SerializeField] private float attackInterval = 1f; // Zeit zwischen Angriffen
    [SerializeField] private GameObject projectilePrefab; // Projektil, das der Turm abfeuert
    [SerializeField] private Transform firePoint; // Punkt, von dem aus das Projektil abgefeuert wird
    [SerializeField] private LayerMask enemyLayer; // Layer der Gegner

    private float attackCooldown = 0f; // Zeit bis zum nächsten Angriff

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        // Gegner in Reichweite finden
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (enemiesInRange.Length > 0)
        {
            Transform closestEnemy = GetClosestEnemy(enemiesInRange);

            if (closestEnemy != null && attackCooldown <= 0f)
            {
                Attack(closestEnemy);
                attackCooldown = attackInterval; // Cooldown zurücksetzen
            }
        }
    }

    private Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }

    private void Attack(Transform enemy)
    {
        // Projektil abfeuern
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            //Setze enemy als Target von Projectile
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectileScript.SetTarget(enemy);
            }
            
        }
    }

    void OnDrawGizmosSelected()
    {
        // Zeichnet die Reichweite des Turms im Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}