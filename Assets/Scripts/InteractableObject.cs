using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractableObject : MonoBehaviour
{
    #region Variables
    public bool equipable;                          // Tell me if I can equip or not the object

    [SerializeField] private GameObject button;     // Ref to the showed text    
    [SerializeField] private KeyCode interactKeyCode;    
    [SerializeField] Vector3 offsetEquip;           // Local offset applied to the equipable object
    public KeyCode InteractKeyCode => interactKeyCode;

    private Collider trigger;                       // Ref to the Object Collider    
    private Animator anim;                          // Ref to the Object Animator
    private Animator animButton;                    // Ref. to the Canvas Animator
    private PlayerActions playerActions;            // Ref. to the 'PlayerActions' script from Player    
    private Transform pivotWeapon;    
    #endregion

    #region Unity API
    void Awake()
    {
        trigger = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        animButton = button.GetComponentInParent<Animator>();
        playerActions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
        pivotWeapon = playerActions.pivotWeapon;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))        
            PlayerInRange(true, this);

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))        
            PlayerInRange(false, null);
    }
    #endregion
    #region Private Methods
    private void PlayerInRange(bool state, InteractableObject _object)
    {
        button.SetActive(state);
        animButton.SetBool("ShowUp", state);        
        playerActions.InteractableObj = _object;
        playerActions.CanInteract = state;
    }
    private void IsTheObjectAnObstacle()
    {
        // If the GO of this script has a NavMeshObstacle Component then we'll disable it
        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if(obstacle)
            obstacle.enabled = false;
    }
    public void SetButtonAndCollider(bool state)
    {
        // Disable the Trigger Collider and the Canvas Text
        trigger.enabled = state;        
        button.SetActive(state);
    }
    #endregion
    #region Public Methods    
    public void Interact()
    {
        // Plays the Opening Chest Animation
        anim.SetTrigger("Interact");
        SetButtonAndCollider(false);
        //this.enabled = false;             // Disable This script
        IsTheObjectAnObstacle();
    }   
    public void Equip()
    {
        SetButtonAndCollider(false);
        transform.SetParent(pivotWeapon);   // Set the current GO as a child of Pivot Weapon
        transform.localPosition = offsetEquip;
        transform.localRotation = Quaternion.identity;  
    }
    #endregion
}
