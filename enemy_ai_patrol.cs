using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_ai_patrol : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    BoxCollider boxCollider;
    public EnemyHealth enemyhealth;
    [SerializeField] float damage;

    [SerializeField] LayerMask groundlayer, playerLayer;

    // Patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    [SerializeField] float patrolCooldown = 5f; // If stuck, retry patrol after 5s
    float patrolTimer;

    // State change
    [SerializeField] float sightRange, attackRange, rangeAttackRange;
    bool playerInSight, playerInRangeAttack, playerInAttackRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        patrolTimer = patrolCooldown;
        enemyhealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInRangeAttack = Physics.CheckSphere(transform.position, rangeAttackRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange) Patrol();
        if (playerInSight && !playerInRangeAttack&& !playerInAttackRange) Chase();
        if (playerInSight && playerInRangeAttack && !playerInAttackRange) RangeAttack();
        if (playerInSight && playerInAttackRange) Attack();
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void RangeAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Armature_Armature_mixamo_com_Layer0_003"))
        {
            animator.SetTrigger("RangedAttack");
            agent.SetDestination(transform.position);
        }
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Armature_Armature_mixamo_com_Layer0_002"))
        {
            animator.SetTrigger("Attack");
            agent.SetDestination(transform.position);
        }
    }

    void Patrol()
    {
        if (!walkpointSet) SearchForDestination();
        if (walkpointSet) agent.SetDestination(destPoint);

        // Check if the AI is stuck and retry patrol
        if (Vector3.Distance(transform.position, destPoint) < 2f || agent.velocity.magnitude < 0.1f)
        {
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0f)
            {
                walkpointSet = false;
                patrolTimer = patrolCooldown;
            }
        }
    }

    void SearchForDestination()
    {
        for (int i = 0; i < 10; i++) // Try up to 10 times to find a valid point
        {
            float randomZ = Random.Range(-range, range);
            float randomX = Random.Range(-range, range);
            Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                destPoint = hit.position;
                walkpointSet = true;
                return;
            }
        }

        // If no valid point found, reset and try again later
        walkpointSet = false;
    }

    void EnableAttack()
    {
        boxCollider.enabled = true;
    }

    void DisableAttack()
    {
        boxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Skip if it's not on the Player layer
        if (((1 << other.gameObject.layer) & playerLayer) == 0)
            return;

        // Prevent hitting yourself
        if (other.transform.root == transform.root)
            return;

        var player = other.GetComponent<player_movement>();
        if (player != null)
        {
            player.health.HP -= damage;
            if (player.health.HP <= 0)
            {
                Destroy(player.gameObject);
            }
        }
    }
}