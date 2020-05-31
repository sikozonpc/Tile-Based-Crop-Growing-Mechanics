using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Utils;

namespace Gameplay
{
	public class CameraController : MonoBehaviour
	{
		[Header("Sprites")]
		public Tile selectionTileSprite;

		// 2. TODO: - Crop growing stages.

		[Header("HUD Element")]
		public Text selectedTileText;
		public Image selectedTileImage;

		private IGameTile selectedTile;
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
						TileController.instance.PlaceTile(mouseWorldPos, previewTile.name, tilemapLayers.GetCurrentSelectedLayer());
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
			IGameTile tile = TileController.GetTileByAssetName(tileID);
			SetPreviewTile(tile.TileData);
		}

		private void UpdatedSelectedTileHUD(IGameTile tile)
		{
			selectedTileText.text = tile.Description;
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

			print("Cliked at " + worldPoint);

			// Tries to access Dictionary key
			if (TileController.instance.tiles.TryGetValue(worldPoint, out selectedTile))
			{
				print("Tile " + selectedTile.Description + " costs: " + selectedTile.ID);
				SetSelectedTile(selectedTile);
				UpdatedSelectedTileHUD(selectedTile);
			}
		}

		private void SetSelectedTile(IGameTile tile)
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