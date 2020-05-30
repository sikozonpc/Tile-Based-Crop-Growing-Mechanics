using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Utils;

namespace Gameplay
{
	public class CameraController : MonoBehaviour
	{
		public Tile selectionTileSprite;

		// 2. TODO: - Crop growing stages.

		public Text selectedTileText;
		public Image selectedTileImage;

		private GameTile selectedTile;
		private Tile previewTile;
		private Vector3Int previousTileCoordinate;


		private TileController tileController;
		private TilemapLayerController tilemapLayers;

		private void Start()
		{
			tileController = FindObjectOfType<TileController>();
			tilemapLayers = FindObjectOfType<TilemapLayerController>();

			previewTile = selectionTileSprite;
		}

		private void Update()
		{
			Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			if (!HUDController.IsPointerOverUIElement())
			{
				AddSelectionSpriteToTile(mouseWorldPos);

				if (Input.GetMouseButtonDown(0) && previewTile)
				{
					if (previewTile.name != selectionTileSprite.name)
					{
						TileController.instance.PlaceTile(mouseWorldPos, previewTile, tilemapLayers.GetCurrentSelectedLayer());
						SetPreviewTile(selectionTileSprite); // deselect the preview tile
					}
				}
			} else
			{
				DeselectAllSelectionTiles();
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SetPreviewTile(selectionTileSprite);
			}

			if (Input.GetMouseButtonDown(0))
			{
				OnWorldTileSelect(mouseWorldPos);
			}

			if (Input.GetKeyDown(KeyCode.G))
			{
				tileController.GenerateMap();
			}
		}

		public void SelectTileFromHUD(string tileID)
		{
			GameTile tile = TileController.GetTileByID(tileID);

			//SetSelectedTile(tile);
			SetPreviewTile(tile.TileData);
			//UpdatedSelectedTileHUD(tile);
		}

		private void UpdatedSelectedTileHUD(GameTile tile)
		{
			selectedTileText.text = tile.Name;
			selectedTileImage.sprite = tile.TileData.sprite;
		}

		private void AddSelectionSpriteToTile(Vector3 mousePos)
		{
			Tilemap selectionTilemap = tilemapLayers.SelectionLayer.tilemap;

			Vector3Int tileCoordinate = selectionTilemap.WorldToCell(mousePos);

			if (tileCoordinate != previousTileCoordinate)
			{
				selectionTilemap.SetTile(previousTileCoordinate, null);
				selectionTilemap.SetTile(tileCoordinate, previewTile);
				previousTileCoordinate = tileCoordinate;
			}
		}


		private void OnWorldTileSelect(Vector3 mousePos)
		{
			int currentSelectedlayer = tilemapLayers.GetCurrentSelectedLayer().layer;

			Vector3Int worldPoint = new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), currentSelectedlayer);

			var tiles = TileController.instance.tiles;

			print("Cliked at " + worldPoint);

			// Tries to access Dictionary key
			if (tiles.TryGetValue(worldPoint, out selectedTile))
			{
				print("Tile " + selectedTile.Name + " costs: " + selectedTile.Cost);
				SetSelectedTile(selectedTile);
				UpdatedSelectedTileHUD(selectedTile);

				//selectedTile.TilemapMember.SetTileFlags(selectedTile.LocalPlace, TileFlags.None);
				//selectedTile.TilemapMember.SetColor(selectedTile.LocalPlace, Color.green);
			}
		}

		private void SetSelectedTile(GameTile tile)
		{
			selectedTile = tile;
		}

		private void DeselectAllSelectionTiles()
		{
			tilemapLayers.SelectionLayer.tilemap.ClearAllTiles();
		}

		private void SetPreviewTile(Tile tile)
		{
			previewTile = tile;
		}
	}
}