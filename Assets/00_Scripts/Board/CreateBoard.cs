using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CreateBoard : MonoBehaviour
{
    private int row;
    private int column;
    [SerializeField] private Vector3 offset;
   
    private TileColor tileColor;
    Dictionary<string, GameObject> _dicTiles = new Dictionary<string, GameObject>();
    public List<GameObject> boards = new List<GameObject>();
    public BoardData boardData;

    void OnDestroy()
    {
        boards.Clear();
        _dicTiles.Clear();
    }

    async void Awake()
    {        
        _dicTiles = Resources.LoadAll<GameObject>("Board").ToDictionary(t => t.name.ToLower(), t => t);
        await LoadBoardData();
    }

    void Start()
    {
        row = GameManager.intance.row;
        column = GameManager.intance.column;
        SpawnBoard();
    }

    private void SpawnBoard()
    {
        for (int i = 0; i < row * column; i++)
        {
            tileColor = (i % column + i / column) % 2 == 0 ? TileColor.normal : TileColor.highlight;
            GameObject g = ObjectPooling.CreateGameObject(tileColor.ToString(), _dicTiles[tileColor.ToString()]);
            g.transform.SetParent(this.transform);
            RectTransform rect = g.GetComponent<RectTransform>();
            Vector2Int index = new Vector2Int(i / column, i % column);
            g.GetComponent<SelectTile>().index = index;
            rect.localScale = Vector3.one;
            Vector3 pos;
            if (i == 0)
            {
                Sprite img = g.GetComponent<Image>().sprite;
                offset = new Vector3(rect.rect.width * rect.localScale.x, rect.rect.height * rect.localScale.y, 0);
                pos = Vector3.zero;
            }
            else if (i % column == 0)
            {
                pos = boards[i - column].GetComponent<RectTransform>().localPosition - new Vector3(0, offset.y, 0);
            }
            else
            {
                pos = boards[i - 1].GetComponent<RectTransform>().localPosition + new Vector3(offset.x, 0, 0);
            }
            rect.localPosition = pos;
            boards.Add(g);
            GameManager.intance.index.Add(index, g);
        }
        Vector3 fit = boards[row * column - 1].GetComponent<RectTransform>().localPosition / 2;
        for (int i = 0; i < row * column; i++)
        {
            boards[i].GetComponent<RectTransform>().localPosition -= fit;
            LoadDataInIndex( new Vector2Int(i / column , i % column), boards[i]);
        }
    }

    public enum TileColor
    {
        normal,
        highlight
    }

    private Task LoadBoardData()
    {
        string json = PlayerPrefs.GetString(Contant.BoardData, "");
        if (string.IsNullOrEmpty(json))
            return Task.CompletedTask;
        boardData = JsonUtility.FromJson<BoardData>(json);
        return Task.CompletedTask;
    }

    public void LoadDataInIndex(Vector2Int index, GameObject parent)
    {
        string color = boardData.getTileDataInIndex(index);
        if (string.IsNullOrEmpty(color))
        {
            return;
        }
        GameManager.intance.squareColors.TryGetValue(color, out Sprite squareColor);
        if (squareColor == null) return;
        Block block = Factory.createBlock("Block");
        GameObject square = block.SpawnBlock(Vector3.zero, squareColor, parent, Vector3.one);
        square.GetComponent<BoxCollider2D>().enabled = false;
        parent.GetComponent<SelectTile>().shape = square;
    }
}
