using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Patrol")]
    [SerializeField] Transform[] checkPoints;
    private int index;                          // Patrol Index position
    private int navMeshPatrolMask;              // Patrol Navigation Mask

    [Header("Alert")]
    [SerializeField] float visionAngle;
    [SerializeField] float visionRange;
    [SerializeField] float visionHeight;
    [SerializeField] private bool playerDetected;
    public bool PlayerDetected 
    {  
        get { return playerDetected; } 
        set { playerDetected = value; } 
    }

    [Header("Speed")]
    [SerializeField] float patrolSpeed;
    [SerializeField] float attackingSpeed;

    [Header("Attack")]
    [SerializeField] Collider colliderAttack;
    [SerializeField] float waitingTimeToAttack;
    [SerializeField] float cadenceAttack;

    // GO Components
    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;
    #endregion

    #region Unity API
    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        // Set the Mask to apply to the enemy's patrol        
        navMeshPatrolMask = NavMeshAreas.Walkable;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        PatrolAndAlert(true);
    }

    void Update()
    {
        Animating();   
    }
    #endregion

    #region Private Methods
    private void Animating()
    {
        anim.SetFloat("Velocity", agent.velocity.magnitude);
    }
    private IEnumerator Patrol()
    {
        agent.speed = patrolSpeed;

        #region Patrol by CheckPoints
        while (true)
        {
            agent.SetDestination(checkPoints[index].position);
            // Loops here till you reach the destination
            while (Vector3.Distance(transform.position, checkPoints[index].position) > agent.stoppingDistance)            
                yield return null;
            // Choose a new patrol point (after a random elapsed time)
            yield return new WaitForSeconds(Random.Range(1, 3));
            index++;
            if(index >= checkPoints.Length)
                index = 0;
        }
        #endregion

        #region Patrol Random
        //Vector3 destination = transform.position;
        //while (true)
        //{
        //    // Remains here till the new destination is reached
        //    while (Vector3.Distance(transform.position, destination) > agent.stoppingDistance)
        //    {
        //        yield return null;
        //        // If the speed is pretty small I reached my destination (To avoid an infinite loop)
        //        if (agent.velocity.magnitude < 0.1f)
        //        {
        //            destination = transform.position;
        //        }
        //    }                

        //    // Random Waiting Time
        //    yield return new WaitForSeconds(Random.Range(1, 3));

        //    // Get a Random point within a Sphere of Radius 20
        //    Vector3 randomPoint = transform.position + (Random.insideUnitSphere*5);
        //    // Find the nearest point from our random point and set it as the new destination
        //    NavMeshHit hit;
        //    NavMesh.SamplePosition(randomPoint, out hit, 10, navMeshPatrolMask);
        //    destination = hit.position;
        //    agent.SetDestination(destination);  
        //}        
        #endregion
    }
    private IEnumerator Alert()
    {
        while (true)
        {
            Vector3 direction = player.position - transform.position;
            float distance = Vector3.Distance(transform.position, player.position);
            float angle = Vector3.Angle(transform.forward, direction);
            float diffY = Mathf.Abs(transform.position.y - player.position.y);            

            // The condition to detect the player:
            if (distance < visionRange && angle < visionAngle && diffY < visionHeight && !playerHealth.IsDead)
            {
                // I go for the player
                // playerDetected = true;
                Attacking(true);
                PatrolAndAlert(false);
            }
            yield return null;
        }
    }
    private IEnumerator Attack()
    {
        playerDetected = true;          // Set to true the player Detected flag

        agent.speed = attackingSpeed;
        agent.ResetPath();
        anim.Play("Alert");

        yield return new WaitForSeconds(waitingTimeToAttack);

        //Debug.Log("Waiting for Alert Anim");
        //yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Alert"));
        //Debug.Log("Alert Anim Done");

        while (true)
        {
            // Loops here as long as the player is too far from the enemy to attack him
            while(Vector3.Distance(transform.position,player.position) > agent.stoppingDistance)
            {
                // If the player is out of the enemy's vision range
                // --> Stop pursuing the player, Reset the Patrol and Alert Coroutines and exit this one.
                if (Vector3.Distance(transform.position, player.position) > visionRange * 2)
                {
                    // Came back to the Patrol
                    playerDetected = false;
                    PatrolAndAlert(true);
                    Attacking(false);                    
                }                
                // The enemy will constantly pursue to the player as long as he's within his vision range
                else
                {                    
                    agent.SetDestination(player.position);
                }
                yield return null;
            }

            // Once the enemy is close enough to attack the player (and the player is still alive)
            // he'll execute periodically the Attack animation
            if (!playerHealth.IsDead)
            {                
                transform.LookAt(player.position);
                anim.Play("Attack");                
            }
            // If the player dies we'll exit this coroutine and came back to the Patrol and Alert ones 
            else
            {
                // Came back to the Patrol
                playerDetected = false;
                PatrolAndAlert(true);
                Attacking(false);
            }
            yield return new WaitForSeconds(cadenceAttack);
        }
    }
    private void OnDrawGizmos()
    {
        // Draw a red sphere which represents the Enemy's vision range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Draw 2 green raycast which represents the Enemy's vision angle
        Gizmos.color = Color.green;
        // Gets the 2 vectors rotating around the Y axis +-visionAngle from transform.forward direction
        Vector3 rightDir = Quaternion.Euler(0, visionAngle, 0) * transform.forward;
        Vector3 leftDir = Quaternion.Euler(0, -visionAngle, 0) * transform.forward;
        // Draws the 2 Gizmos taking the enemy's position (+1m offset on Y).
        Gizmos.DrawRay(transform.position + new Vector3(0,1,0), rightDir * visionRange);
        Gizmos.DrawRay(transform.position + new Vector3(0,1,0), leftDir * visionRange);
    }
    // Methods to be called as event from the Attack Animation
    private void ActivateCollider()
    {
        Debug.Log("Enable Collider");
        colliderAttack.enabled = true;
    }
    private void DeactivateCollider()
    {
        Debug.Log("Disable Collider");
        colliderAttack.enabled = false;
    }
    #endregion

    #region Public Methods
    public void PatrolAndAlert(bool state)
    {
        if (state)
        {
            StartCoroutine(nameof(Patrol));
            StartCoroutine(nameof(Alert));
        }
        else
        {
            StopCoroutine(nameof(Patrol));
            StopCoroutine(nameof(Alert));
        }
    }
    public void Attacking(bool state)
    {
        if (state)
            StartCoroutine(nameof(Attack));
        else
            StopCoroutine(nameof(Attack));
    }
    #endregion
}
