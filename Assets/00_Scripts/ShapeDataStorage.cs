using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeDataStorage : Singleton<ShapeDataStorage>
{
    public List<ShapeData> shapeDatas = new List<ShapeData>();
    public override void Awake()
    {
        base.Awake();
        shapeDatas = Resources.LoadAll<ShapeData>("ShapeData").ToList();
    }

    public int LoadNewShapeData()
    {
        if (shapeDatas.Count == 0) return -1;
        int index = Random.Range(0, shapeDatas.Count);
        return index;
    }

}
