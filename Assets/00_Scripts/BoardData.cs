using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BoardData
{
    public int row;
    public int column;
    public TileData[] boardData;


    public void SaveDataBoard()
    {
        row = GameManager.intance.row;
        column = GameManager.intance.column;
        boardData = new TileData[row * column];
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                Vector2Int idx = new Vector2Int(r, c);
                boardData[r * column + c] = new TileData();
                boardData[r * column + c].setTileData(idx);
            }
        }
    }

    public string getTileDataInIndex(Vector2Int index)
    {
        if (!PlayerPrefs.HasKey("BoardData")) return "";
        string color = "";
        if (index.x < 0 || index.x >= row || index.y < 0 || index.y >= column)
        {
            Debug.LogError("Index out of bounds");
            return color;
        }
        TileData tileData = boardData[index.x * column + index.y];
        color = tileData.colorName;
        return color;
    }
}

[System.Serializable]
public class TileData
{
    public string colorName = "";
    public void setTileData(Vector2Int idx)
    {
        SelectTile tile = GameManager.intance.index[idx].GetComponent<SelectTile>();
        if (tile.shape == null)
        {
            return;
        }
        colorName = tile.shape.GetComponent<Image>().sprite.name.ToLower();
    }
}
