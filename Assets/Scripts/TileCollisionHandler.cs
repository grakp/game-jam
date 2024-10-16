using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class TileCollisionHandler : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase sceneChangeTile;
    public string sceneName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(collision.transform.position);
        TileBase collidedTile = tilemap.GetTile(cellPosition);

        if (collidedTile == sceneChangeTile)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
