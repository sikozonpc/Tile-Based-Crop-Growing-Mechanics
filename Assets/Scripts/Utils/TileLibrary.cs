using UnityEngine;
using System.Collections.Generic;
using Gameplay;
using UnityEngine.Tilemaps;
using System;

public class TileLibrary : MonoBehaviour
{
    public static TileLibrary instance;
    public Dictionary<string, IGameTile> Tiles;

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

        Tiles = new Dictionary<string, IGameTile>();

        InitLibrary();
    }

    public IGameTile GetClonedTile(string id)
    {
        IGameTile originalTile = Tiles[id];
        IGameTile clonedTile = (IGameTile)originalTile.Clone();
        clonedTile.ID = Guid.NewGuid().ToString();

        return clonedTile;
    }

    private void InitLibrary()
    {
        Tiles.Add("tomato_001", new CropTile()
        {
            Description = "Tomato seed - Stage 1",
            TileBase = Resources.Load<TileBase>("Sprites/tomato_001"),  // location to the art sprites
            TileData = Resources.Load<Tile>("Sprites/tomato_001"),
            GrowthStageTiles = new GrowthStage[]
            {
                 new GrowthStage() {
                    Description = "Tomato root - Stage 2",
                    Tile = Resources.Load<Tile>("Sprites/tomato_002"),
                },
                 new GrowthStage() {
                    Description = "Tomato root - Stage 3",
                    Tile = Resources.Load<Tile>("Sprites/tomato_003"),
                },
                new GrowthStage() {
                    Description = "Tomato big root - Stage 4",
                    Tile = Resources.Load<Tile>("Sprites/tomato_004"),
                },
                new GrowthStage() {
                    Description = "Tomato big root - Stage 5",
                    Tile = Resources.Load<Tile>("Sprites/tomato_005"),
                },
                new GrowthStage() {
                    Description = "Grown tomato",
                    Tile = Resources.Load<Tile>("Sprites/tomato_006"),
                },
            },
            GrowthTime = 2,
        });
        Tiles.Add("grass_001", new GameTile()
        {
            Description = "Grass",
            TileBase = Resources.Load<TileBase>("Sprites/grass_001"),
            TileData = Resources.Load<Tile>("Sprites/grass_001"),
        });
    }
}
