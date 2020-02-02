﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// NOTE: Before building the game (when we're finished), change the 'individual' rendering mode to 'chunk' and make a sprite atlas instead.
// This will fix/make more efficient isometric rendering in the final build; this 'individual' rendering mode is just a temporary way to 
// get the tilemap to sort correctly while still being able to make/edit/add new tiles into the tile palette.

public class BuildMode : MonoBehaviour
{
    public Tile chube;
    public Tile buildProcessTile;
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public BuildCursor cursor;
    public Materials materials;
    public BuildButtonsController controller;

    void Update()
    {
        // Cursor color is red by default
        cursor.render.color = Color.red;
        Vector3Int cellPosition = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        cellPosition.z = tilemapRenderer.sortingOrder;

        // If current cell is available, set cursor to green
        if (checkAvailability(cellPosition))
        {
            cursor.render.color = Color.green;

            // If it's available and the player clicks the mouse, build a floor tile there
            if (Input.GetButton("Fire1") && materials.amount >= controller.cost)
            {
                // create coroutine of building
                
                Tile tile = controller.currentTile;
                materials.amount -= controller.cost;
                tilemap.SetTile(cellPosition, buildProcessTile);
                StartCoroutine(buildNewTile(tile, cellPosition, controller.buildTime));
            }
        }        
    }

    // Checks if any of the surrounding 4 tiles contains a floor
    private bool checkAvailability(Vector3Int pos)
    {
        if (tilemap.HasTile(pos) || materials.amount < controller.cost) return false;

        ArrayList borderTiles = new ArrayList();
        borderTiles.Add(new Vector3Int(pos.x, pos.y + 1, pos.z));
        borderTiles.Add(new Vector3Int(pos.x, pos.y - 1, pos.z));
        borderTiles.Add(new Vector3Int(pos.x + 1, pos.y, pos.z));
        borderTiles.Add(new Vector3Int(pos.x - 1, pos.y, pos.z));

        foreach (Vector3Int borderTile in borderTiles)
        {
            if (tilemap.HasTile(borderTile)) return true;
        }

        return false;
    }

    IEnumerator buildNewTile(Tile tile, Vector3Int pos, float time)
    {
        bool done = false;
        if (done) yield break;

        yield return new WaitForSecondsRealtime(time);
        tilemap.SetTile(pos, tile);

        done = true;
    }
}