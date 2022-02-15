using System.Collections.Generic;
using UnityEngine;
using static PrefabsDatabase;

public class PoolDatabase : MonoBehaviour
{
    private static Dictionary<Prefabs, List<GameObject>> PooledObjects = new Dictionary<Prefabs, List<GameObject>>();

    public static void CreatePool(Prefabs p_id, int p_quantity)
    {
        if(PooledObjects.ContainsKey(p_id))
        {
            Debug.LogError("Pool for " + p_id + " already exists.");
            return;
        }

        List<GameObject> __myPool = new List<GameObject>(p_quantity);

        GameObject __poolable;

        for (int __i = 0; __i < p_quantity; __i++)
        {
            __poolable = InstantiatePrefab(p_id, 0, null);
            __poolable.SetActive(false);
            __myPool.Add(__poolable);
        }

        PooledObjects.Add(p_id, __myPool);
    }

    public static GameObject GetPooledObject(Prefabs p_id)
    {
        List<GameObject> __myPool = PooledObjects[p_id];

        for (int __i = 0, __j = __myPool.Count - 1; __i < __myPool.Count; __i++, __j--)
        {
            if (!__myPool[__i].activeSelf)
            {
                return __myPool[__i];
            }
            else if (!__myPool[__j].activeSelf)
            {
                return __myPool[__j];
            }
        }

        return RaisePoolSupply(p_id);
    }

    public static T GetPooledObject<T>(Prefabs p_id)
    {
        List<GameObject> __myPool = PooledObjects[p_id];

        for (int __i = 0, __j = __myPool.Count - 1; __i < __myPool.Count; __i++, __j--)
        {
            if(!__myPool[__i].activeSelf)
            {
                return __myPool[__i].GetComponent<T>();
            }
            else if (!__myPool[__j].activeSelf)
            {
                return __myPool[__j].GetComponent<T>();
            }
        }

        return RaisePoolSupply(p_id).GetComponent<T>();
    }

    private static GameObject RaisePoolSupply(Prefabs p_id)
    {
        List<GameObject> __myPool = PooledObjects[p_id];

        GameObject __poolable = InstantiatePrefab(p_id, 0, null);
        __poolable.SetActive(false);
        __myPool.Add(__poolable);

        return __poolable;
    }
}
