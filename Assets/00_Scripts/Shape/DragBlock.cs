using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragBlock : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public List<Transform> childs = new List<Transform>();
    RectTransform rect;
    private Vector3 _currentPos;
    public bool canDrop;

    void Start()
    {
        canDrop = false;
        rect = this.GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        childs.Clear();
        _currentPos = rect.localPosition;
        canDrop = true;
        rect.localScale = Vector3.one * 1 / 0.8f;
        Observer.action = () => CheckShape();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        foreach (Transform c in transform)
        {
            childs.Add(c);
            RectTransform rect = c.gameObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = Vector2.one * 0.25f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.localPosition += (Vector3)eventData.delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect.localScale = Vector3.one;
        foreach (var c in childs)
        {
            canDrop &= c.GetComponent<Shape>().canDrop;
            RectTransform rect = c.GetComponent<RectTransform>();
            rect.anchorMin = Vector3.one * 0.5f;
            rect.anchorMax = Vector3.one * 0.5f;
            rect.pivot = Vector3.one * 0.5f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rect.localPosition = _currentPos;
        if (canDrop)
        {
            SetTileParent();
        }
        canDrop = false;
    }

    private void SetTileParent() 
    {
        GenerateShape genShape = GetComponent<GenerateShape>();
        int idx = GameManager.intance.currentShapeData.IndexOf(genShape.shapeData);
        genShape.shapeData = null;
        genShape.idx = -1;
        GameManager.intance.currentShapeData.RemoveAt(idx);
        foreach (var c in childs)
        {
            Shape shape = c.GetComponent<Shape>();
            RectTransform rect = c.GetComponent<RectTransform>();
            Transform parent = c.gameObject.activeSelf ? GameManager.intance.index[shape.index].transform : null;
            c.SetParent(parent, false);
            rect.localPosition = Vector3.zero;
            rect.transform.localScale = Vector3.one;
            c.GetComponent<BoxCollider2D>().enabled = false;
        }
        GameManager.intance.shapeCount -= 1;
        GameManager.intance.GameOver();
    }

    public bool CheckShape()
    {
        foreach (var c in childs)
        {
            Shape s = c.GetComponent<Shape>();
            if ((int)s.state == 1 || s.canDrop == false)
            {
                return false;
            }
        }
        return true;
    }

    
}
