using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    void OnDestroy()
    {
        foreach (var pool in pool)
        {
            foreach (var obj in pool.Value)
            {
                Destroy(obj);
            }
        }
        pool.Clear();
    }

   public static Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

    public static GameObject CreateGameObject(string name, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"Prefab '{name}' is null or destroyed, cannot create pooled object.");
            return null;
        }
        if (!pool.ContainsKey(name))
        {
            pool.Add(name, new List<GameObject>());
        }
        foreach (var obj in pool[name])
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject g = GameObject.Instantiate(prefab);
        pool[name].Add(g);
        return g;
    }
}
