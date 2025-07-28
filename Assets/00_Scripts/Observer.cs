using System;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    static Dictionary<string, List<Action>> listener = new Dictionary<string, List<Action>>();
    public static Func<object> action;
    public static Func<object[], object> checkGameOver;
    public static Func<object , object> writings;

    void OnDestroy()
    {
        listener.Clear();
        action = null;
    }

    public static void AddListener(string name, Action callBack)
    {
        if (!listener.ContainsKey(name))
        {
            listener.Add(name, new List<Action>());
        }
        listener[name].Add(callBack);
    }

    public static void RemoveListener(string name, Action callBack)
    {
        if (!listener.ContainsKey(name))
        {
            return;
        }
        listener[name].Remove(callBack);
    }

    public static void Notify(string name)
    {
        if (!listener.ContainsKey(name)) return;

        foreach (var a in listener[name])
        {
            try
            {
                a?.Invoke(); 
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}
