
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> objectQueue;
    private GameObject prefab;
    private Transform parent;

    private static ObjectPool<T> instance = null;
    public static ObjectPool<T> Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool<T>();
            }
            return instance;
        }
    }

    public int queueCount
    {
        get
        {
            return objectQueue.Count;
        }
    }

    public void InitPool(GameObject prefab, Transform parent = null, int warmUpCount = 0)
    {
        this.prefab = prefab;
        this.parent = parent;
        objectQueue = new Queue<T>();

        List<T> warmUpList = new List<T>();
        for (int i = 0; i < warmUpCount; i++)
        {
            T t = Instance.Spawn(Vector3.zero, Quaternion.identity);
            warmUpList.Add(t);
        }
        for (int i = 0; i < warmUpList.Count; i++)
        {
            Instance.Recycle(warmUpList[i]);
        }
    }

    public T Spawn(Vector3 position, Quaternion quaternion)
    {
        if (prefab == null)
        {
            Debug.LogError(typeof(T).ToString() + " prefab not set!");
            return default(T);
        }
        if (queueCount <= 0)
        {
            GameObject g = Object.Instantiate(prefab, position, quaternion, parent);
            T t = g.GetComponent<T>();
            if (t == null)
            {
                Debug.LogError(typeof(T).ToString() + " not found in prefab!");
                return default(T);
            }
            objectQueue.Enqueue(t);
        }
        T obj = objectQueue.Dequeue();
        obj.gameObject.transform.position = position;
        obj.gameObject.transform.rotation = quaternion;
        obj.gameObject.transform.parent = parent;
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Recycle(T obj)
    {
        objectQueue.Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}