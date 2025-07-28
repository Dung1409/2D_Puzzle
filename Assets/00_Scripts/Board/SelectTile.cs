using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class SelectTile : MonoBehaviour
{
    public GameObject hide;
    public Vector2Int index;
    public GameObject shape;
    public List<Shape> shapes = new List<Shape>();
    public int available { get => transform.childCount == 2 ? 1 : 0; set { } }

    public int value;
    void OnDestroy()
    {
        index = Vector2Int.zero;
        shape = null;
        shapes.Clear();
        available = 0;
    }

    void Awake()
    {
        hide = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        value = available;
    }

    void OnTriggerEnter2D(Collider2D orther)
    {
        if (GameManager.intance.isGameOver) return;
        Shape s = orther.GetComponent<Shape>();
        if (transform.childCount == 2)
        {
            s.SetColor(1);
            return;
        }
        s.SelectedTile(true, this.GetComponent<SelectTile>());
        hide.SetActive(true);
        StartCoroutine(DeLayTriggerCheck());
    }

    void OnTriggerStay2D(Collider2D orther)
    {
        if (GameManager.intance.isGameOver) return;
        Shape s = orther.GetComponent<Shape>();
        if (transform.childCount == 2)
        {
            s.SetColor(1);
            return;
        }
        s.SelectedTile(true, this.GetComponent<SelectTile>());
        hide.SetActive(true);
        StartCoroutine(DeLayTriggerCheck());
    }

    void OnTriggerExit2D(Collider2D orther)
    {
         if (GameManager.intance.isGameOver) return;
        hide.SetActive(false);
        Shape s = orther.GetComponent<Shape>();
        s.canDrop = false;
        s.SetColor(0);
        shape = transform.childCount == 2 ? transform.GetChild(1).gameObject : null;
        ChangeStateOrRemove();
    }
    void ChangeStateOrRemove()
    {
        if (this.gameObject.activeSelf) StartCoroutine(DelayDeactive());
    }

    public void CheckNeighbor(Vector2Int _dir, ref List<Shape> shapes)
    {
        List<Shape> _shapes = new List<Shape>();
        Vector2Int idx = index;
        int value = _dir.x != 0 ? GameManager.intance.row : GameManager.intance.column;
        while (_shapes.Count != value + 1)
        {
            SelectTile t;
            GameManager.intance.index[idx].TryGetComponent<SelectTile>(out t);
            if (t == null || t.shape == null)
            {
                _shapes.Clear();
                return;
            }
            Shape s = t.shape.GetComponent<Shape>();
            _shapes.Add(s);
            idx += _dir;
            if (idx.x >= value) idx.x %= value;
            if (idx.y >= value) idx.y %= value;
        }
        if (Observer.action == null)
        {
            Debug.LogError("Observer.action is null");
            return;
        }
        if (!(bool)Observer.action?.Invoke()) return;
        foreach (var s in _shapes)
        {
            if (!shapes.Contains(s))
            {
                shapes.Add(s);
                s.overrideShape = shape.GetComponent<Shape>().normalShape;
                s.SetColor(2);
            }
        }
    }

    IEnumerator DeLayTriggerCheck()
    {
        yield return null;
        CheckNeighbor(Vector2Int.right, ref shapes);
        CheckNeighbor(Vector2Int.up, ref shapes);
    }

    IEnumerator DelayDeactive()
    {
        yield return null;
        List<Shape> _shapes = new List<Shape>(shapes);
        if (transform.childCount == 2)
        {
            yield return new WaitForEndOfFrame();
            DeactiveShapeInLine(ref _shapes);
        }
        else
        {
            foreach (var c in _shapes)
            {
                c.SetColor(0);
            }
        }
        shapes.Clear();

    }

    public void DeactiveShapeInLine(ref List<Shape> _shapes)
    {
        bool isActive = false;
        int sidx = -1;
        int eidx = -1;
        for (int i = 0; i < _shapes.Count; i++)
        {
            GameObject c = _shapes[i].gameObject;
            if (!c.activeSelf)
            {
                continue;
            }
            if (sidx == -1)
            {
                sidx = i;
            }
            eidx = i;
            isActive = true;
            c.SetActive(false);
            c.transform.SetParent(null);
        }
        if (!isActive) return;
        int lines = 0;
        if (_shapes.Count == GameManager.intance.row + GameManager.intance.column - 1)
        {
            if (sidx < GameManager.intance.row) lines += 1;
            if (eidx >= GameManager.intance.row) lines += 1;
        }
        else if (_shapes.Count == GameManager.intance.row || _shapes.Count == GameManager.intance.column) lines = 1;
        ScoreManager.intance.LinesCleared(lines);
        StartCoroutine(DelayWritings());
    }

    IEnumerator DelayWritings()
    {
        yield return new WaitForEndOfFrame();
        if (ScoreManager.intance.lines > 0)
        {
            Observer.writings?.Invoke(ScoreManager.intance.lines);
            yield return new WaitForSeconds(0.5f);
            ScoreManager.intance.lines = 0;
        }
    }
    
    
}


