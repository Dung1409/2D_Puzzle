using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GenerateShape : MonoBehaviour
{
    GameObject squarePrefab;
    [Header("Shape Data")]
    public ShapeData shapeData;
    public int idx;

    [SerializeField] private Vector3 size = new Vector3(0.8f, 0.8f, 0.8f);
    private Vector2 offset;

    Sprite squareSprite;
    List<Vector3> tilesPos = new List<Vector3>();

    public bool canDrop;

    void Start()
    {
        squarePrefab = GameManager.intance.square;
        RectTransform rect = squarePrefab.GetComponent<RectTransform>();
        offset = new Vector2(rect.rect.width * size.x, rect.rect.height * size.y);
        Observer.AddListener(Contant.GenerateShape, GenShape);
        Observer.AddListener(Contant.SaveData, SaveData);
    }

    void OnDisable()
    {
        Observer.RemoveListener(Contant.SaveData, SaveData);
        Observer.RemoveListener(Contant.GenerateShape, GenShape);
    }

    public void GenShape()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.DetachChildren();
        shapeData = GetShapeData();
        if (shapeData == null) return;
        tilesPos.Clear();
        squareSprite = GameManager.intance.GetSquareColor();

        int r = shapeData.row;
        int c = shapeData.column;
        Vector3 endPos = new Vector3();
        for (int i = 0; i < r * c; i++)
        {
            Vector3 nextPos;
            if (i == 0)
            {
                nextPos = Vector3.zero;
            }
            else if (i % c == 0)
            {
                nextPos = tilesPos[i - c] - new Vector3(0, offset.y, 0);
            }
            else
            {
                nextPos = tilesPos[i - 1] + new Vector3(offset.x, 0, 0);
            }
            tilesPos.Add(nextPos);
            if (shapeData.rows[i / c].columns[i % c]) endPos = nextPos;
        }
        if (endPos == null)
        {
            return;
        }
        Vector3 fit = new Vector3(endPos.x / 2, endPos.y, 0);
        for (int i = 0; i < r * c; i++)
        {
            tilesPos[i] -= fit;
            if (shapeData.rows[i / c].columns[i % c]) SpawnShape(tilesPos[i], squareSprite);
        }
        GameManager.intance.shapeCount = Mathf.Min(GameManager.intance.shapeCount + 1, 3);
        GameManager.intance.GameOver();

    }

    private void SpawnShape(Vector3 pos, Sprite sprite)
    {
        Block block = Factory.createBlock("Block");
        GameObject square = block.SpawnBlock(pos, sprite, this.gameObject, size);
    }

    private ShapeData GetShapeData()
    {
        ShapeData newShapeData = null;
        idx = PlayerPrefs.GetInt(this.gameObject.name, ShapeDataStorage.intance.LoadNewShapeData());
        PlayerPrefs.DeleteKey(this.gameObject.name);
        if (idx == -1)
        {
            return null;
        }
        newShapeData = ShapeDataStorage.intance.shapeDatas[idx];
        GameManager.intance.currentShapeData.Add(newShapeData);
        return newShapeData;
    }

    void SaveData()
    {
        PlayerPrefs.SetInt(this.gameObject.name, idx);
        PlayerPrefs.Save();
    }

}
