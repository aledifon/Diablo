using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{    
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageHealth;
    [SerializeField] private Image healthImage;    

    private float currentHealth;
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    // Flags
    public bool IsDead { get; private set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Update the Player Health if an Enemy Attack is detected (Enemy's collider)
        if (other.CompareTag("DamageEnemy"))
        {
            currentHealth -= damageHealth;
            healthImage.fillAmount = currentHealth/maxHealth;
            if (currentHealth <= 0)
                Death();
        }   
    }
    private void Death()
    {
        IsDead = true;
        playerAttack.enabled = false;   
        playerMovement.enabled = false;
        anim.Play("Death");
    }
}
