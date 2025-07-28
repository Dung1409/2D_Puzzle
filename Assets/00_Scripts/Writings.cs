using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Writings : MonoBehaviour
{
    public List<Sprite> writings = new List<Sprite>();
    private Image _img;
    private Animator _anim;
    int lines;
    public bool isCalling;

    void Start()
    {
        isCalling = false;
        _img = GetComponent<Image>();
        _anim = GetComponent<Animator>();
        writings = Resources.LoadAll<Sprite>("Writings").ToArray().ToList();
        Observer.writings = (lines) =>
        {
            this.lines = (int)lines;
            SetWritings();
            return null;
        };
    }

    public void SetWritings()
    {
        if (isCalling) return;
        isCalling = true;
        if (lines <= 0 || lines >= writings.Count)
        {
            return;
        }
        _img.sprite = writings[lines - 1];
        _anim.SetTrigger("Play");
        isCalling = false;
    }
}
