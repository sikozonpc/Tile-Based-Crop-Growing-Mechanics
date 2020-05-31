using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

/*
   Tile Controller Singleton 
*/

namespace Gameplay
{
	public delegate void PlantPlantedHandler(string ID);

	public class TileController : MonoBehaviour
	{
		public static TileController instance;
		public Dictionary<Vector3, IGameTile> tiles = new Dictionary<Vector3, IGameTile>();

		private TilemapLayerController tilemapLayers;

		public event PlantPlantedHandler OnStageGrow;

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
				IGameTile tileFromLibrary = GetTileByAssetName(tileBase.name);

				IGameTile tile = new GameTile
				{
					LocalPlace = localPlace,
					WorldLocation = layeredWorldPosition,
					TileBase = tileBase,
					TilemapMember = tilemap,
					Description = tileFromLibrary.Description,
					TileData = tileFromLibrary.TileData,
					Cost = 1
				};

				tiles.Add(layeredWorldPosition, tile);
			}
		}

		public Dictionary<Vector3, IGameTile> CreatePopulatedTilemap(TilemapLayer tilemapLayer)
		{
			int layer = tilemapLayer.layer;
			Tilemap tilemap = tilemapLayer.tilemap;
			Dictionary<Vector3, IGameTile> localTilemap = new Dictionary<Vector3, IGameTile>();

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

				IGameTile tileFromLibrary = GetTileByAssetName("grass_001");

				IGameTile tile = new GameTile
				{
					LocalPlace = localPlace,
					WorldLocation = layeredWorldPosition,
					TileBase = tileFromLibrary.TileBase,
					TilemapMember = tilemap,
					Description = tileFromLibrary.Description,
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

			foreach (var tile in groundTiles)
			{
				IGameTile tileData = tile.Value;
				SetGameTile(tilemapLayers.GroundLayer, tileData);
			}
		}

		public void PlaceTile(Vector3 pos, string assetName, TilemapLayer tilemapLayer)
		{
			Tilemap tilemap = tilemapLayer.tilemap;
			int layer= tilemapLayer.layer;

			Vector3Int tilemapPos = tilemap.WorldToCell(pos);
			Vector3 layeredWorldPosition = new Vector3(tilemapPos.x, tilemapPos.y, layer);

			Vector3Int localPlace = new Vector3Int(tilemapPos.x, tilemapPos.y, layer);

			IGameTile newTile = TileLibrary.instance.GetClonedTile(assetName);
			newTile.LocalPlace = localPlace;
			newTile.WorldLocation = layeredWorldPosition;
			newTile.TilemapMember = tilemap;

			// if a tile already exists there, just replace it.
			bool tileExistsInPos = tiles.ContainsKey(layeredWorldPosition);
			if (tileExistsInPos)
			{
				tiles[layeredWorldPosition] = newTile;
			} else
			{
				tiles.Add(layeredWorldPosition, newTile);
			}

			bool isACrop = newTile.GetType() == typeof(CropTile);
			if (isACrop)
			{
				(newTile as CropTile).StartGrowing();
			}

			SetGameTile(tilemapLayer, newTile);
		}


		// Starts a coroutine and returns to the caller after it's time is passed
		public void Grow(int timeToGrow, int stages, string ID)
		{
			StartCoroutine(StartGrowing(timeToGrow, stages, ID));
		}

		private IEnumerator StartGrowing(int timeToGrow, int stages, string ID)
		{
			for (int stage = 0; stage < stages; stage++)
			{
				yield return new WaitForSeconds(timeToGrow);
				OnStageGrow?.Invoke(ID);
			}
		}

		private void SetGameTile(TilemapLayer tilemapLayer, IGameTile gameTile)
		{
			tilemapLayer.tilemap.SetTile(gameTile.LocalPlace, gameTile.TileBase);
		}

		public static IGameTile GetTileByAssetName(string assetName)
		{
			return TileLibrary.instance.GetClonedTile(assetName);
		}
	}
}