using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Flags")]
    [SerializeField] private bool canMove;           // Tell me if the player can move
    public bool CanMove => canMove;         

    [Header("Components")]
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Raycast")]
    private Ray ray;
    private RaycastHit hit;

    [Header("Player Movement")]
    private bool move;              // Indicates the player is going to move
    #endregion

    #region Unity API
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();        
    }
    private void Update()
    {
        InputPlayer();
        Move();
        Rotate();
        Animating();
    }
    #endregion

    #region Private Methods
    private void InputPlayer()
    {
        if(Input.GetMouseButtonDown(0) && canMove)
            move = true;
    }
    private void Move()
    {
        if (move)
        {
            move = false;

            // Raycast setup
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast Launch
            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
    private void Rotate()
    {
        //Debug.Log("Quaternion: " + transform.rotation);
        //Debug.Log("Euler: " + transform.eulerAngles);        
        if (agent.velocity != Vector3.zero)
        {            
            transform.eulerAngles = new Vector3(0, Quaternion.LookRotation(agent.velocity).eulerAngles.y, 0);
        }
    }    
    private void Animating()
    {
        anim.SetFloat("Velocity",agent.velocity.magnitude);
    }
    private void UpdatePlayerMovement(bool enableState)
    {
        canMove = enableState;
    }
    #endregion

    #region Public Methods
    public void EnablePlayerMovement()
    {
        UpdatePlayerMovement(true);
    }
    public void DisablePlayerMovement()
    {
        UpdatePlayerMovement(false);
    }
    #endregion
}
