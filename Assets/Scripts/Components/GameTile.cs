using System;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay
{
    public interface IGameTile : ICloneable
    {
        string ID { get; set; }
        Vector3Int LocalPlace { get; set; }
        Vector3 WorldLocation { get; set; }
        TileBase TileBase { get; set; }
        Tilemap TilemapMember { get; set; }
        string Description { get; set; }
        int Cost { get; set; }
        Tile TileData { get; set; }
    }

    public class GameTile : IGameTile, ICloneable
    {
        public string ID { get; set; }
        public Vector3Int LocalPlace { get; set; }
        public Vector3 WorldLocation { get; set; }
        public TileBase TileBase { get; set; }
        public Tilemap TilemapMember { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        public Tile TileData { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public Sprite GetTileSprite()
        {
            return TilemapMember.GetSprite(LocalPlace);
        }
    }
}

