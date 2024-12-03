using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 50;
    public Transform baseTarget;
    public UnityAction OnDeath;

    private GameLogic gameLogic;

private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        gameLogic = FindObjectOfType<GameLogic>();
        baseTarget = GameObject.Find("Base").transform;
    }

        void Update()
    {
        // Wenn Basis zerstört oder nicht vorhanden, Bewegung stoppen
        if (baseTarget == null)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        // Erneut Ziel setzen, falls Agent hängen bleibt
        if (!navMeshAgent.hasPath && navMeshAgent.remainingDistance <= 0.1f)
        {
            navMeshAgent.SetDestination(baseTarget.position);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(); // Event auslösen
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Base"))
        {
            if (gameLogic != null)
            {
                gameLogic.TakeDamage(10); // Basis Schaden zufügen
            }

            Destroy(gameObject);
        }
    }
}
