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
		public Tilemap selectionTilemap;
		private int selectionLayer = 1;

		//
		// 1: TODO: - Create structured layer with index and tilemap together.
		//          - Selection layer automatically selects tilemap to select the tile to.

		// 2. TODO: - Crop growing stages.

		public Tilemap objectsTilemap;

		public Text selectedTileText;
		public Text selectionLayerText;
		public Image selectedTileImage;

		private GameTile selectedTile;
		private Tile previewTile;
		private TileController tileController;
		private Vector3Int previousTileCoordinate;


		private void Start()
		{
			tileController = FindObjectOfType<TileController>();

			previewTile = selectionTileSprite;
			selectionLayerText.text = selectionLayer.ToString();
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
						TileController.instance.PlaceTile(mouseWorldPos, previewTile, objectsTilemap, selectionLayer);
						SetPreviewTile(selectionTileSprite); // deselect it know
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

			if (Input.GetKeyDown(KeyCode.Home))
			{
				IncreaseSelectionLayer();
			}
			if (Input.GetKeyDown(KeyCode.End))
			{
				DecreaseSelectionLayer();
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
			Vector3Int worldPoint = new Vector3Int(Mathf.FloorToInt(mousePos.x), Mathf.FloorToInt(mousePos.y), selectionLayer);

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
			selectionTilemap.ClearAllTiles();
		}

		private void SetPreviewTile(Tile tile)
		{
			previewTile = tile;
		}

        #region Debug Tools

		private void IncreaseSelectionLayer()
		{
			selectionLayer = selectionLayer + 1;
			selectionLayerText.text = selectionLayer.ToString();
		}
		private void DecreaseSelectionLayer()
		{
			selectionLayer = selectionLayer - 1;
			selectionLayerText.text = selectionLayer.ToString();
		}


		#endregion
	}
}