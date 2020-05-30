using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
using Utils;

/*
   Tile Controller Singleton 
*/

namespace Gameplay
{
	public class TileController : MonoBehaviour
	{
		public static TileController instance;
		public Dictionary<Vector3, GameTile> tiles = new Dictionary<Vector3, GameTile>();

		private TilemapLayerController tilemapLayers;

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

			tilemapLayers = FindObjectOfType<TilemapLayerController>();
		}

		private void ReadTilemapToTileData(Tilemap tilemap, int layer)
		{
			foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
			{
				Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

				if (!tilemap.HasTile(localPlace)) continue;

				var worldLocation = tilemap.CellToWorld(localPlace);
				var layeredWorldPosition = new Vector3(worldLocation.x, worldLocation.y, layer);

				TileBase tileBase = tilemap.GetTile(localPlace);
				GameTile tileFromLibrary = GetTileByID(tileBase.name);

				GameTile tile = new GameTile
				{
					LocalPlace = localPlace,
					WorldLocation = layeredWorldPosition,
					TileBase = tileBase,
					TilemapMember = tilemap,
					Name = tileFromLibrary.Name,
					TileData = tileFromLibrary.TileData,
					Cost = 1
				};

				print(layeredWorldPosition);
				tiles.Add(layeredWorldPosition, tile);
			}
		}

		public Dictionary<Vector3, GameTile> CreatePopulatedTilemap(TilemapLayer tilemapLayer)
		{
			int layer = tilemapLayer.layer;
			Tilemap tilemap = tilemapLayer.tilemap;
			Dictionary<Vector3, GameTile> localTilemap = new Dictionary<Vector3, GameTile>();

			foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
			{
				Vector3Int localPlace = new Vector3Int(pos.x, pos.y, layer);

				bool isTileEmpty = !tilemap.HasTile(localPlace);

				if (!isTileEmpty)
				{
					print("Tile at: " + localPlace + " already exists!");
					continue;
				}

				var worldLocation = tilemap.CellToWorld(localPlace);
				var layeredWorldPosition = new Vector3(worldLocation.x, worldLocation.y, layer);

				GameTile tileFromLibrary = GetTileByID("grass_001");

				GameTile tile = new GameTile
				{
					LocalPlace = localPlace,
					WorldLocation = layeredWorldPosition,
					TileBase = tileFromLibrary.TileBase,
					TilemapMember = tilemap,
					Name = tileFromLibrary.Name,
					Cost = 0,
					TileData = tileFromLibrary.TileData,
				};

				localTilemap.Add(layeredWorldPosition, tile);
			}

			return localTilemap;
		}

		public void GenerateMap()
		{
			var groundTiles = CreatePopulatedTilemap(tilemapLayers.GroundLayer);
			// Adds ground tiles to the global tiles
			tiles = DataUtils.MergeDictionaries(tiles, groundTiles);

			//var objectTiles = CreatePopulatedTilemap(objectsTilemap, 1);
			// Adds object tiles to the global tiles
			//tiles = DataUtils.MergeDictionaries(tiles, objectTiles);


			foreach (var tile in groundTiles)
			{
				GameTile tileData = tile.Value;
				SetGameTile(tilemapLayers.GroundLayer, tileData);
			}
		}

		public void PlaceTile(Vector3 pos, Tile tile, TilemapLayer tilemapLayer)
		{
			Tilemap tilemap = tilemapLayer.tilemap;
			int layer= tilemapLayer.layer;

			Vector3Int tilemapPos = tilemap.WorldToCell(pos);
			Vector3 worldLocation = tilemap.CellToWorld(tilemapPos);
			Vector3 layeredWorldPosition = new Vector3(worldLocation.x, worldLocation.y, layer);

			Vector3Int localPlace = new Vector3Int(tilemapPos.x, tilemapPos.y, layer);

			print(localPlace);

			GameTile newTile = new GameTile()
			{
				LocalPlace = localPlace,
				WorldLocation = layeredWorldPosition,
				TileBase = tile,
				TilemapMember = tilemap,
				Name = tile.name,
				Cost = 0,
				TileData = tile,
			};

			// if a tile already exists there, just replace it.
			bool tileExistsInPos = tiles.ContainsKey(layeredWorldPosition);

			print(tileExistsInPos);

			if (tileExistsInPos)
			{
				tiles[layeredWorldPosition] = newTile;
			} else
			{
				tiles.Add(layeredWorldPosition, newTile);
			}
			
			SetGameTile(tilemapLayer, newTile);
		}


		private void SetGameTile(TilemapLayer tilemapLayer, GameTile gameTile)
		{
			tilemapLayer.tilemap.SetTile(gameTile.LocalPlace, gameTile.TileBase);
		}

		public static GameTile GetTileByID(string id)
		{
			return TileLibrary.instance.Tiles[id];
		}
	}
}