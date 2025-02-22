using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float currentHealth;
    [SerializeField] float maxHealth;
    [SerializeField] private float damageHealth;
    [SerializeField] Slider slider;

    // GO Components
    private NavMeshAgent agent;
    private Animator anim;
    private Enemy enemy;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("FireBall"))
        {
            // If the Player has not been detected then the Enemy will directly pass to 'Attacking' State 
            if(enemy.PlayerDetected == false)
            {                
                enemy.Attacking(true);
                enemy.PatrolAndAlert(false);
            }

            // Gets the FireBall Prefab component
            FireBall fireBallPrefab = collision.collider.gameObject.GetComponent<FireBall>();

            // Update Enemy's Health
            currentHealth -= damageHealth;
            slider.value = currentHealth;
                      
            // Returns the FireBall Prefab to the Object Pool
            if (fireBallPrefab.gameObject.activeInHierarchy)
                ObjectPool.SharedInstance.ReturnToPool(fireBallPrefab);
            //Destroy(collision.gameObject);

            // Executes the Death Method
            if (currentHealth <= 0)
                Death();
        }
    }
    private void Death()
    {
        // Stops all the Enemy Coroutines
        enemy.Attacking(false);
        enemy.PatrolAndAlert(false);

        // Disable the Nav Mesh Agent
        agent.enabled = false;                      
                
        // Plays the Death Animation
        anim.Play("Death");        
    }

    private void DisableSlider()
    {
        slider.gameObject.SetActive(false);
    }
}
