using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class PlayerAttack : MonoBehaviour
{
    #region Variables
    [Header ("Fireball Settings")]
    [SerializeField] Rigidbody fireballPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootForce;

    // GO Components    
    private Animator anim;
    private PlayerMovement playerMovement;          // In order to access to PlayerMovement.canMove
    private NavMeshAgent agent;

    // Raycast vars
    private Ray ray;
    private RaycastHit hit;

    // IObjectPool variables
    //IObjectPool<GameObject> m_Pool;
    //public IObjectPool<ParticleSystem> Pool
    //{
    //    get
    //    {
    //        if (m_Pool == null)
    //        {                
    //            m_Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);                
    //        }
    //        return m_Pool;
    //    }
    //}
    #endregion

    #region Events & Delegates
    #endregion

    #region Unity API
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(1)) 
            Attack();
    }
    #endregion

    #region Private Methods
    private void Attack()
    {
        // Play the Attack animation
        anim.Play("Attack");    
        // Blocks any player movement + reset the player velocity +
        // Reset any possible Path on running
        playerMovement.CanMove = false;
        agent.velocity = Vector3.zero;    
        agent.ResetPath();
    }
    #endregion

    #region Public Methods
    // Method call from the Animation (Animation Event)
    public void Shoot()
    {
        // GO Cloning through Object Pooling Design Pattern

        // Gets a non active FireBallPrefab from the Object Pool
        GameObject cloneFireballPrefab = ObjectPool.SharedInstance.GetPooledObject();
        // Sets its Transform
        cloneFireballPrefab.transform.position = shootPoint.position;
        cloneFireballPrefab.transform.rotation = shootPoint.rotation;    
        // Adds Force to the Prefab Rb Component
        cloneFireballPrefab.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shootForce);

        //Rigidbody cloneFireballPrefab = Instantiate(fireballPrefab, 
        //                                        shootPoint.position,
        //                                        shootPoint.rotation);
        //cloneFireballPrefab.AddForce(shootPoint.forward * shootForce);

        //// Destroy the Clone after 4s
        //Destroy(cloneFireballPrefab, 4);
    }
    #endregion
}
