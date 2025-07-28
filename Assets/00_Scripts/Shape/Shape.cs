using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public Vector2Int index;
    public bool canDrop;
    public State state;

    [Header("Image State")]
    private Image _img;
    public Sprite normalShape;
    public Sprite errorShape;
    public Sprite overrideShape;
    void Awake()
    {
        _img = GetComponent<Image>();
        normalShape = _img.sprite;
        state = State.normalShape;
    }
    void OnEnable()
    {
        canDrop = false;
        this.GetComponent<BoxCollider2D>().enabled = true;
        try
        {
            index = GetComponentInParent<SelectTile>().index;
        }
        catch (System.Exception)
        {
            index = Vector2Int.zero;
        }
    }

    void OnDisable()
    {
        
        GameObject tile = GameManager.intance.index[index];
        if (tile.Equals(this.transform.parent.gameObject))
        {
            ScoreManager.intance.UpdateScore(5);
            SelectTile s = tile.GetComponent<SelectTile>();
            s.shape = null;
        }
        
    }
    public void SelectedTile(bool selected, SelectTile collision)
    {
        canDrop = selected;
        collision.shape = this.gameObject;
        index = collision.index;
    }
    public void SetColor(int _state)
    {
        state = (State)_state;
        _img.sprite = state != State.normalShape ? (state == State.errorShape ? errorShape : overrideShape) : normalShape;
    }

    public enum State
    {
        normalShape,
        errorShape,
        overrideShape
    }    
}
