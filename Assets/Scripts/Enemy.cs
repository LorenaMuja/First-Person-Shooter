using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent enemyNavMeshAgent;
    public Animator enemyAnimator;
    public int damage;
    private int health;
    public Slider enemyHealthSlider;
    public int maxHealth;
    public float hidingDuration;
    private float hidingTimer;
    public float knockBackDuration;
    private float knockBackTimer;
    private bool isKnockBackFinished = true;
    public Transform[] waypoints;
    public float detectionRange;
    public float waypointRadius;
    public float runningSpeedThreshold;
    private int nextWaypointIndex;
    private bool isFollowingPlayer;
    public float timeBetweenHits;
    private float hittingTimer;

    void Start()
    {
        health = maxHealth;
        MoveToNextWaypoint();
    }

    public void Hit(int damage)
    {
        knockBackTimer = knockBackDuration;
        FinishKnockBack();
        isKnockBackFinished = false;
        health -= damage;
        enemyHealthSlider.value = (float)health / maxHealth;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (knockBackTimer > 0)
            knockBackTimer -= Time.deltaTime;
        else
        {
            if (isKnockBackFinished == false)
            {
                FinishKnockBack();
                isKnockBackFinished = true;
            }

            Move();
        }

        if (enemyNavMeshAgent.velocity.sqrMagnitude > runningSpeedThreshold * runningSpeedThreshold)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }

        if (hidingTimer > 0)
        {
            hidingTimer -= Time.deltaTime;
        }
        else
        {
            HideHealthBar();
        }

        if (hittingTimer > 0)
            hittingTimer -= Time.deltaTime;
    }

    private void Move()
    {      
        if (!IsPlayerInRange() && !isFollowingPlayer)
            Patrol();
        else 
            FollowPlayer();
    }

    private void Patrol()
    {
        if (HasReachedCurrentWaypoint())
            MoveToNextWaypoint();
    }

    private void FollowPlayer()
    {
        if (!isFollowingPlayer)
        {
        isFollowingPlayer = true;
            enemyNavMeshAgent.speed = 15;
        }
        if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 100, NavMesh.AllAreas))
        {
            enemyNavMeshAgent.destination = hit.position;
            Debug.DrawRay(enemyNavMeshAgent.destination, Vector3.up * 100, Color.red);
        }
    }

    private bool IsPlayerInRange()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        return Vector3.SqrMagnitude(toPlayer) < detectionRange * detectionRange;
    }

    private bool HasReachedCurrentWaypoint()
    {
        int currentWaypointIndex = nextWaypointIndex - 1;
        currentWaypointIndex = (currentWaypointIndex + waypoints.Length) % waypoints.Length;
        Vector3 toDestination = waypoints[currentWaypointIndex].position - transform.position;
        return toDestination.sqrMagnitude < waypointRadius * waypointRadius;
    }

    private void MoveToNextWaypoint()
    {
        enemyNavMeshAgent.destination = waypoints[nextWaypointIndex].position;
        ++nextWaypointIndex;
        nextWaypointIndex %= waypoints.Length;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player && hittingTimer <= 0)
        {
            hittingTimer = timeBetweenHits;           
            player.GetComponent<PlayerHealth>().Hit(damage);
        }
    }

    public void ShowHealthBar()
    {
        enemyHealthSlider.gameObject.SetActive(true);
        hidingTimer = hidingDuration;
    }

    private void HideHealthBar()
    {
        enemyHealthSlider.gameObject.SetActive(false);

    }
    public void FinishKnockBack()
    {
        enemyAnimator.enabled = true;
        enemyNavMeshAgent.enabled = true;
    }
}
