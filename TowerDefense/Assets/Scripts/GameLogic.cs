using System.Collections;
using UnityEngine;
using TMPro;

public class GameLogic : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text roundText;         // Anzeige der aktuellen Runde
    [SerializeField] private TMP_Text enemiesText;       // Anzeige der verbleibenden Gegner
    [SerializeField] private TMP_Text playerHealthText;  // Anzeige der Lebenspunkte des Spielers

    [Header("Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab; // Prefab der Gegner
    [SerializeField] private Transform[] spawnPoints; // Spawnpunkte der Gegner
    [SerializeField] private int baseEnemiesPerRound = 3; // Basisanzahl an Gegnern pro Runde
    [SerializeField] private float spawnInterval = 1f; // Zeit zwischen einzelnen Spawns
    [SerializeField] private float timeBetweenRounds = 5f; // Wartezeit zwischen den Runden

    [Header("Player Settings")]
    [SerializeField] private int playerHealth = 100; // Lebenspunkte des Spielers

    private int currentRound = 0; // Aktuelle Runde
    private int remainingEnemies = 0; // Verbleibende Gegner in der aktuellen Runde

    void Start()
    {
        // Initiale Werte in der UI anzeigen
        UpdateRoundUI(0);
        UpdateEnemiesCountUI(0);
        UpdatePlayerHealthUI();

        // Spawning-Routine starten
        StartCoroutine(SpawnRounds());
    }

    IEnumerator SpawnRounds()
    {
        while (playerHealth > 0) // Spiel läuft, solange Spieler noch Leben hat
        {
            currentRound++;
            remainingEnemies = baseEnemiesPerRound + (currentRound - 1) * 2;

            // Runde und Gegneranzahl in der UI aktualisieren
            UpdateRoundUI(currentRound);
            UpdateEnemiesCountUI(remainingEnemies);

            Debug.Log($"Runde {currentRound} gestartet! Gegneranzahl: {remainingEnemies}");

            for (int i = 0; i < remainingEnemies; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                // Ziel der Gegner setzen
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.baseTarget = FindObjectOfType<Base>().transform;
                    enemyScript.OnDeath += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(spawnInterval);
            }

            // Wartezeit zwischen den Runden
            yield return new WaitForSeconds(timeBetweenRounds);
        }

        // Spiel-Ende
        Debug.Log("Spiel beendet!");
    }

    private void HandleEnemyDeath()
    {
        remainingEnemies--;

        // UI aktualisieren
        UpdateEnemiesCountUI(remainingEnemies);

        // Überprüfen, ob alle Gegner besiegt wurden
        if (remainingEnemies <= 0)
        {
            Debug.Log($"Runde {currentRound} abgeschlossen!");
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        // Spieler-Lebenspunkte in der UI aktualisieren
        UpdatePlayerHealthUI();

        // Überprüfen, ob Spieler noch lebt
        if (playerHealth <= 0)
        {
            Debug.Log("Spieler ist gestorben!");
            StopAllCoroutines(); // Gegner-Spawning stoppen
        }
    }

    // UI-Methoden
    private void UpdateRoundUI(int round)
    {
        if (roundText != null)
        {
            roundText.text = $"Runde: {round}";
        }
    }

    private void UpdateEnemiesCountUI(int count)
    {
        if (enemiesText != null)
        {
            enemiesText.text = $"Gegner übrig: {count}";
        }
    }

    private void UpdatePlayerHealthUI()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = $"Leben: {playerHealth}";
        }
    }
}