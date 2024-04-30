//Script for respawning the same objects over and over such as runes and fireballs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ObjectPooler : MonoBehaviour
{
    static public ObjectPooler Instance { get; private set; }

    [Header("Object-type parents")]
    [SerializeField] Transform spellParent;

    [Header("Pooled objects")]
    [SerializeField] List<Pools> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    List<string> objectsOfType;    

    //Setup pooling
    private void Start()
    {
        Instance = this;
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
                    if (p.type == PoolType.Rune)
                    {
                        obj.transform.SetParent(LevelManager.pointer.transform);
                        obj.transform.localScale = new Vector3(4, 4, 0.01f);//Create and set new material

                        //Material mat = 
                        obj.transform.GetComponent<UnityEngine.UI.Image>().material = Material.Instantiate(p.material);
                    }
                    else
                    {
                        obj.transform.SetParent(transform);
                    }
                }

                poolDictionary.Add(p.name, q);
            }
        }
    }

    #region Retrieve from Object Pooling
    public GameObject SpawnRandomFromType(PoolType type, Vector3 position)
    {
        objectsOfType = new List<string>();
        foreach (Pools p in pools) {
            if (p.type == type) { objectsOfType.Add(p.name); }
        }
        return SpawnPooledObject(objectsOfType[Random.Range(0, objectsOfType.Count - 1)], position);
    }

    public GameObject SpawnPooledObject(string name, Vector3 position)
    {
        //Check to see if item exists
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pooled object "+name+" doesn't exist!");
            return null;
        }

        //Spawn in object from pool
        GameObject spawned = poolDictionary[name].Dequeue();
        spawned.SetActive(true);

        if (spawned.GetComponent<RectTransform>() != null)
        {
            spawned.GetComponent<RectTransform>().position = position;
        }
        else
        {
            spawned.transform.position = position;
        }
        if (spawned.GetComponent<EnemyRune>() != null)
        {
            spawned.GetComponent<EnemyRune>().OnStart();
        }

        //Re-queue object
        poolDictionary[name].Enqueue(spawned);
        return spawned;
    }
    #endregion
}

//Pooling structs to spawn in and use pooled objects
[System.Serializable]
public class Pools
{
    public string name;
    public PoolType type = PoolType.PhysicalObject;
    public GameObject pooledObject;
    public int size;
    public Material material;
}

//Types of objects in pools used for differet behaviours
[System.Serializable]
public enum PoolType
{
    Rune,
    Spell,
    PhysicalObject
}