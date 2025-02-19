using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    #region Variables
    private static ObjectPool sharedInstance;
    public static ObjectPool SharedInstance { get { return sharedInstance; } }

    private IObjectPool<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int initialPoolSize = 10;
    [SerializeField] private int maxPoolSize = 20;
    #endregion

    #region Unity API
    void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        // Inicializar el ObjectPool con los métodos de gestión
        pooledObjects = new ObjectPool<GameObject>(
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
    private GameObject CreatePooledItem()
    {
        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
    #endregion

    #region Public Methods    
    // Método público para obtener un objeto del pool
    public GameObject GetPooledObject()
    {
        return pooledObjects.Get();
    }

    // Método público para devolver un objeto al pool
    public void ReturnToPool(GameObject obj)
    {
        pooledObjects.Release(obj);
    }
    #endregion
}
