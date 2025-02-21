using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    #region Variables
    [Header ("Fireball Lifetime")]    
    [SerializeField] private float lifeTime;

    private Rigidbody rb;
    #endregion

    #region Unity API
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }
    #endregion


    #region Private Methods
    // Method called once the Prefab instance is enabled
    private void OnEnable()
    {
        StartCoroutine(nameof(ReturnToPoolAfterTime));
    }

    // Return to the Pool Object the current GO Prefab after an elapsed time
    private IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject.activeInHierarchy)
            ObjectPool.SharedInstance.ReturnToPool(this);
    }
    #endregion

    #region Public Methods
    public void LaunchFireball(Vector3 shootPointForward, float shootForce)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(shootPointForward * shootForce);
    }
    #endregion
}
