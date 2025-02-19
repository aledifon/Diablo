using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header ("Fireball Lifetime")]    
    [SerializeField] private float lifeTime;

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
            ObjectPool.SharedInstance.ReturnToPool(gameObject);
    }
}
