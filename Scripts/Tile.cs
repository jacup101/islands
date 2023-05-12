using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse Entered!");
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse Exited");
        _highlight.SetActive(false);
    }
}
