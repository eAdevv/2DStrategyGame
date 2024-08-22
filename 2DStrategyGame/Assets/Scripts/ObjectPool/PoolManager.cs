using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    Dictionary<string, GameObject> poolPrefabs = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> poolParents = new Dictionary<string, GameObject>();
    Dictionary<string, Queue<ObjectInstance>> poolFreeDictionary = new Dictionary<string, Queue<ObjectInstance>>();
    Dictionary<string, Dictionary<int, ObjectInstance>> poolUsedDictionary = new Dictionary<string, Dictionary<int, ObjectInstance>>();

    public void CreatePool(string poolKey, GameObject prefab, int poolSeedSize)
    {
        if (!poolPrefabs.ContainsKey(poolKey))
        {
            poolPrefabs.Add(poolKey, prefab);

            if (!prefab.GetComponent<PoolObject>())
            {
                prefab.AddComponent<PoolObject>();
            }

            prefab.GetComponent<PoolObject>().poolKey = poolKey;

            poolFreeDictionary.Add(poolKey, new Queue<ObjectInstance>());
            poolUsedDictionary.Add(poolKey, new Dictionary<int, ObjectInstance>());

            Debug.Log(poolKey);

            GameObject poolParent;

            if (poolParents.ContainsKey(poolKey))
            {
                poolParent = poolParents[poolKey];
            }
            else
            {
                poolParent = this.gameObject;
                poolParent.transform.parent = transform;
                poolParents.Add(poolKey, poolParent);
            }

            for (int i = 0; i < poolSeedSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolFreeDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolParent.transform);
            }
        }
    }

    public GameObject GetObject(string poolKey, Vector3 position, Quaternion rotation)
    {
        if (poolPrefabs.ContainsKey(poolKey))
        {
            if (poolFreeDictionary[poolKey].Count > 0)
            {
                ObjectInstance obj = poolFreeDictionary[poolKey].Dequeue();
                poolUsedDictionary[poolKey].Add(obj.gameObject.GetInstanceID(), obj);
                obj.Awake(position, rotation);
                return obj.gameObject;
            }
            else
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(poolPrefabs[poolKey]) as GameObject);
                poolUsedDictionary[poolKey].Add(newObject.gameObject.GetInstanceID(), newObject);
                newObject.SetParent(poolParents[poolKey].transform);
                newObject.Awake(position, rotation);
                return newObject.gameObject;
            }
        }
        return null;
    }

    public void ReturnObjectToQueue(GameObject gameObject)
    {
        if (gameObject.GetComponent<PoolObject>())
        {
            string poolKey = gameObject.GetComponent<PoolObject>().poolKey;
            gameObject.SetActive(false);
            ObjectInstance obj = poolUsedDictionary[poolKey][gameObject.GetInstanceID()];
            poolUsedDictionary[poolKey].Remove(gameObject.GetInstanceID());
            poolFreeDictionary[poolKey].Enqueue(obj);
        }
    }

    private class ObjectInstance
    {
        public GameObject gameObject;
        Transform transform;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);
            poolObjectScript = gameObject.GetComponent<PoolObject>();
        }

        public void Awake(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
            poolObjectScript.OnAwake();
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}