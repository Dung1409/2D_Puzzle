
using UnityEngine;
using UnityEngine.UI;

public class Block
{
    private GameObject squarePrefab { get => GameManager.intance.square; set { } }

    public GameObject SpawnBlock(Vector3 pos, Sprite sprite , GameObject parent , Vector3 size)
    {
        GameObject square = ObjectPooling.CreateGameObject(squarePrefab.name, squarePrefab);
        Image img = square.GetComponent<Image>();
        img.sprite = sprite;
        square.transform.SetParent(parent.transform);
        square.GetComponent<RectTransform>().localPosition = pos;
        square.GetComponent<RectTransform>().localScale = size;
        square.TryGetComponent<Shape>(out Shape s);
        if (square.GetComponent<Shape>() == null)
        {
            s = square.AddComponent<Shape>();
        }
        s.normalShape = sprite;
        s.errorShape = GameManager.intance.errorShape;
        return square;
    }
}
