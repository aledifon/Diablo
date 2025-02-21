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
    [SerializeField] bool playerDetected;        

    // GO Components
    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
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

    private void PatrolAndAlert(bool state)
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

    private IEnumerator Patrol()
    {
        #region Patrol by CheckPoints
        //while (true)
        //{
        //    agent.SetDestination(checkPoints[index].position);
        //    // Loops here till you reach the destination
        //    while (Vector3.Distance(transform.position, checkPoints[index].position) > agent.stoppingDistance)            
        //        yield return null;
        //    // Choose a new patrol point (after a random elapsed time)
        //    yield return new WaitForSeconds(Random.Range(1, 3));
        //    index++;
        //    if(index >= checkPoints.Length)
        //        index = 0;
        //}
        #endregion

        #region Patrol Random
        Vector3 destination = transform.position;
        while (true)
        {
            // Remains here till the new destination is reached
            while (Vector3.Distance(transform.position, destination) > agent.stoppingDistance)
            {
                yield return null;
                // If the speed is pretty small I reached my destination (To avoid an infinite loop)
                if (agent.velocity.magnitude < 0.1f)
                {
                    destination = transform.position;
                }
            }                

            // Random Waiting Time
            yield return new WaitForSeconds(Random.Range(1, 3));

            // Get a Random point within a Sphere of Radius 20
            Vector3 randomPoint = transform.position + (Random.insideUnitSphere*5);
            // Find the nearest point from our random point and set it as the new destination
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint, out hit, 10, navMeshPatrolMask);
            destination = hit.position;
            agent.SetDestination(destination);  
        }        
        #endregion
    }

    private IEnumerator Alert()
    {
        Vector3 direction = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);
        float angle = Vector3.Angle(transform.forward, direction);  
        float diffY = Mathf.Abs(transform.position.y-player.position.y);

        // The condition to detect the player:
        if(distance < visionRange && angle < visionAngle && diffY < visionHeight)
        {
            // I go for the player
            PatrolAndAlert(false);
        }
        yield return null;
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
    #endregion

    #region Public Methods
    #endregion

}
