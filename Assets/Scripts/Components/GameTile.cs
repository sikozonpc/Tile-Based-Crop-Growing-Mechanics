using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Gameplay
{
    public class GameTile
    {
        public Vector3Int LocalPlace { get; set; }

        public Vector3 WorldLocation { get; set; }

        public TileBase TileBase { get; set; }

        public Tilemap TilemapMember { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public string TileIconAssetLocation { get; set; }

        public Tile TileData { get; set; }


        public Sprite GetTileSprite()
        {
            return TilemapMember.GetSprite(LocalPlace);
        }
    }
}

