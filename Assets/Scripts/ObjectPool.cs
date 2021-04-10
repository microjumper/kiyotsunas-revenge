using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int size;

    private List<GameObject> pool;

    void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);

            obj.transform.SetParent(gameObject.transform);

            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < size; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].transform.rotation = rotation;
                pool[i].SetActive(true);

                return pool[i];
            }
        }

        return null;
    }
}