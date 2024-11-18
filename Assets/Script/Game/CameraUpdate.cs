using System;
using UnityEngine;
public class CamareUpdate : MonoBehaviour
{
    private GameAsset gameAsset;

    public void Start()
    {
        gameAsset = GameManager.GameAsset;
    }

    private void Update()
    {
        if (gameAsset.cameraMoveVector2 == Vector2.zero)
            return;
        var dir = new Vector3(gameAsset.cameraMoveVector2.x, 0, gameAsset.cameraMoveVector2.y);
        transform.position += dir * (gameAsset.cameraMoveSpeed * Time.deltaTime);
    }
}