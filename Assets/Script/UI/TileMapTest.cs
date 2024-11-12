using System.Collections.Generic;
using UnityEngine;

public class TileMapTest : MonoBehaviour
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public Color errorColor;

    public Color safeColor;
    
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        _spriteRenderer.color = gameObjects.Count == 0 ? safeColor : errorColor;
    }
    private void OnTriggerEnter(Collider other)
    {
        gameObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObjects.Remove(other.gameObject);
    }
}