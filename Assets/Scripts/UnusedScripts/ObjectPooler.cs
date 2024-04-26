//Script for respawning the same objects over and over such as runes and fireballs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject UIPoolParent;
    [SerializeField] GameObject SceneObjectPoolParent;

    [SerializeField] List<Pools> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public GameObject SpawnNewNote(string name, Vector3 position)
    {
        //Debug.Log("Spawn note thing of name "+name);
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.Log("Obj doesn't exist");
            return null;
        }
        GameObject spawned = poolDictionary[name].Dequeue();

        spawned.SetActive(true);
        spawned.GetComponent<RectTransform>().position = position;

        poolDictionary[name].Enqueue(spawned);
        return spawned;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pools p in pools)
        {
            if (p.size > 0)
            {
                Queue<GameObject> q = new Queue<GameObject>();

                for (int i = 0; i < p.size; i++)
                {
                    GameObject obj = Instantiate(p.pooledObject);
                    obj.SetActive(false);
                    q.Enqueue(obj);
                    if (p.type == PoolType.UI)
                    {
                        obj.transform.SetParent(UIPoolParent.transform);
                    }
                }

                poolDictionary.Add(p.name, q);
            }
        }
    }
}

[System.Serializable]
public class Pools
{
    public string name;
    public PoolType type = PoolType.SceneObject;
    public GameObject pooledObject;
    public int size;
}

[System.Serializable]
public enum PoolType
{
    UI,
    SceneObject
}