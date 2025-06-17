using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Chasing,
        Attacking,
        None
    }

    private State state;

    private EnemyPathfinding enemyPathfinding;
    private Transform player;

    [Header("Player Health")]
    [SerializeField] private HealthBar playerHealthBar;

    [Header("Enemy Stats")]
    public int maxHealth = 3;
    public int health = 3;
    private EnemyHealthBarSpawner healthBarSpawner;
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float damage = 10f;
    [SerializeField] int lootMultiplier = 1;
    private float lastAttackTime;

    [Header("UI & FX")]
    public GameObject floatingPoints;
    public TMP_Text popUpText;
    public GameObject popUpDamagePrefab;
    public Animator animatorSlime;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private BoxCollider2D damageCollider;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        animatorSlime = GetComponent<Animator>();
        damageCollider = GetComponent<BoxCollider2D>();

        if (damageCollider != null)
        {
            damageCollider.isTrigger = true;
            damageCollider.enabled = false; // Start disabled
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;

            if (playerHealthBar == null)
            {
                playerHealthBar = GetComponent<HealthBar>();
            }
        }

        state = State.Roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private void Update()
    {
        if (state == State.None || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (state)
        {
            case State.Roaming:
                if (distanceToPlayer < detectionRange)
                {
                    state = State.Chasing;
                }
                break;

            case State.Chasing:
                if (distanceToPlayer > detectionRange)
                {
                    state = State.Roaming;
                    StartCoroutine(RoamingRoutine());
                }
                else if (distanceToPlayer <= attackRange)
                {
                    state = State.Attacking;
                }
                else
                {
                    enemyPathfinding.MoveTo(player.position);
                }
                break;

            case State.Attacking:
                if (distanceToPlayer > attackRange)
                {
                    state = State.Chasing;
                }
                else
                {
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        animatorSlime.SetTrigger("Attack");
                        lastAttackTime = Time.time;
                        StartCoroutine(EnableDamageCollider());
                    }
                }
                break;
        }
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamOffset = GetRoamingPosition() * 2f;
            Vector2 targetPosition = (Vector2)transform.position + roamOffset;
            enemyPathfinding.MoveTo(targetPosition);
            yield return new WaitForSeconds(2f);
        }

        enemyPathfinding.MoveTo(transform.position);
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        popUpText.text = amount.ToString();
        Instantiate(floatingPoints, transform.position, Quaternion.identity);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }

        healthBarSpawner?.UpdateHealthBar(health, maxHealth);
    }

    private IEnumerator Die()
    {
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                for (int i = 0; i < lootMultiplier; i++)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                }
            }
        }

        animatorSlime.SetTrigger("IsDead");
        state = State.None;
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator EnableDamageCollider()
    {
        yield return new WaitForSeconds(0.3f); // Attack wind-up

        if (damageCollider != null)
            damageCollider.enabled = true;

        yield return new WaitForSeconds(0.2f); // Active damage window

        if (damageCollider != null)
            damageCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (damageCollider.enabled && other.CompareTag("Player"))
        {
            DealDamageToPlayer();
        }
    }

    public void DealDamageToPlayer()
    {
        if (playerHealthBar == null)
        {
            playerHealthBar = GetComponent<HealthBar>();
        }

        if (playerHealthBar != null)
        {
            playerHealthBar.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("PlayerHealthBar is null even after FindObjectOfType.");
        }
    }
}
