using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    #region Variables
    private static ObjectPool sharedInstance;
    public static ObjectPool SharedInstance { get { return sharedInstance; } }

    private IObjectPool<FireBall> pooledObjects;
    [SerializeField] private FireBall objectToPool;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxPoolSize = 20;
    #endregion

    #region Unity API
    void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        // Inicializar el ObjectPool con los métodos de gestión
        pooledObjects = new ObjectPool<FireBall>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true, // collectionCheck (evita liberar objetos dos veces)
            initialPoolSize,
            maxPoolSize
        );
    }
    #endregion

    #region Private Methods
    private FireBall CreatePooledItem()
    {
        Debug.Log(" Creando un nuevo FireBall en el Pool");
        FireBall obj = Instantiate(objectToPool);
        obj.gameObject.SetActive(false);
        Debug.Log(" Objeto creado y desactivado es " + obj);
        return obj;
    }

    private void OnTakeFromPool(FireBall obj)
    {
        obj.gameObject.SetActive(true);
        Debug.Log(" Objeto activado es " + obj);
    }

    private void OnReturnedToPool(FireBall obj)
    {
        obj.gameObject.SetActive(false);
        Debug.Log(" Objeto desactivado es " + obj);
    }

    private void OnDestroyPoolObject(FireBall obj)
    {
        Destroy(obj.gameObject);
        Debug.Log(" Objeto destruido es " + obj);
    }
    #endregion

    #region Public Methods    
    // Método público para obtener un objeto del pool
    public FireBall GetPooledObject()
    {        
        return pooledObjects.Get();
    }

    // Método público para devolver un objeto al pool
    public void ReturnToPool(FireBall obj)
    {
        pooledObjects.Release(obj);
    }
    #endregion
}
