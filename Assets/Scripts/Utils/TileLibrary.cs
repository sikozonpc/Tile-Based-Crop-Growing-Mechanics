using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.UIElements;

public class TileLibrary : MonoBehaviour
{
    public static TileLibrary instance;
    public Dictionary<string, GameTile> Tiles;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Tiles = new Dictionary<string, GameTile>();

        InitLibrary();
        print("Initialized Tile Library, with " + Tiles.Count + " tiles!");
    }

    private void InitLibrary()
    {
        Tiles.Add("tomato_001", new GameTile() { 
            Name = "Tomato seed - Stage 0",
            TileBase = Resources.Load<TileBase>("Sprites/tomato_001"),
            TileData = Resources.Load<Tile>("Sprites/tomato_001"),
        });
        Tiles.Add("tomato_002", new GameTile()
        {
            Name = "Tomato seed - Stage 1",
            TileBase = Resources.Load<TileBase>("Sprites/tomato_002"),
            TileData = Resources.Load<Tile>("Sprites/tomato_002"),
        });
        Tiles.Add("grass_001", new GameTile()
        {
            Name = "Grass",
            TileBase = Resources.Load<TileBase>("Sprites/grass_001"),
            TileData = Resources.Load<Tile>("Sprites/grass_001"),
        });
    }
}
