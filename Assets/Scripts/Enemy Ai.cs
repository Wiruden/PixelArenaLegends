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
    public GameObject popUpDamagePrefab;
    public TMP_Text popUpText;
    public Animator animatorSlime;
    public GameObject floatingPoints;

    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] int damage = 1;
    [SerializeField] int lootmultiplier = 1;
    private float lastAttackTime;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        animatorSlime = GetComponent<Animator>();
        state = State.Roaming;

        // Assume player is tagged "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
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
                        AttackPlayer();
                        lastAttackTime = Time.time;
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

    public int health = 3;

    public void TakeDamage(int amount)
    {
        health -= amount;
        popUpText.text = amount.ToString();
        Instantiate(floatingPoints, transform.position, Quaternion.identity);
        Debug.Log("Enemy took damage! Health: " + health);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                for (int i = 0; i < lootmultiplier; i++)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                }
            }
        }

        animatorSlime.SetTrigger("IsDead");
        Debug.Log("Enemy defeated!");
        GetComponent<EnemyAI>().enabled = false;
        state = State.None;
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

    private void AttackPlayer()
    {
        Debug.Log("Enemy attacks the player for " + damage + " damage!");
        animatorSlime.SetTrigger("Attack");
    }
}
