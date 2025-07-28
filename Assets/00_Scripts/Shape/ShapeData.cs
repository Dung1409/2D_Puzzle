using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeData", menuName = "ShapeData", order = 0)]
[System.Serializable]
public class ShapeData : ScriptableObject
{

    [System.Serializable]
    public class Row
    {
        public bool[] columns;
        public int size;

        public Row() { }

        public Row(int _size)
        {
            size = _size;
            columns = new bool[size];
        }
    }

    public int row;
    public int column;
    public Row[] rows;
    public ShapeData() { }

    public List<Vector2Int> offsets;
    public void CreateNewBoard()
    {
        rows = new Row[row];
        for (int i = 0; i < row; i++)
        {
            rows[i] = new Row(column);
        }
    }

    public List<Vector2Int> Offsets()
    {
        List<Vector2Int> offsets = new List<Vector2Int>();
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                if(rows[r].columns[c] == true) offsets.Add(new Vector2Int(r, c));
            }
        }
        return offsets;
    } 
}
