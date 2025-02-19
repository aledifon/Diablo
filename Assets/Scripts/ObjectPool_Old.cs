using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Old : MonoBehaviour
{
    #region Variables
    private static ObjectPool_Old sharedInstance;
    public static ObjectPool_Old SharedInstance { get { return sharedInstance; } }
    private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    #endregion

    #region Unity API
    void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    #endregion

    #region Private Methods
    #endregion

    #region Public Methods
    public GameObject GetPooledObject()
    {
        // Returns the 1st non-active GO from the Pool        
        foreach (GameObject pooledObject in pooledObjects)
        {
            if (!pooledObject.activeInHierarchy)
            {
                pooledObject.SetActive(true);           // Set it to true before return the Pooled GO
                return pooledObject;
            }
        }

        // TO AVOID RETURN null
        // If doesn't exist any non-active GO on the Pool
        // --> Create it and add it to the Pool
        GameObject newGo = new GameObject();
        newGo.SetActive(true);
        pooledObjects.Add(newGo);        
        return newGo;
        //return null;
    }
    public void ReturnToPool(GameObject obj)
    {
        // If the object belongs to the Pool it will be disabled
        if (pooledObjects.Contains(obj))
            obj.SetActive(false);
    }
    #endregion
}
