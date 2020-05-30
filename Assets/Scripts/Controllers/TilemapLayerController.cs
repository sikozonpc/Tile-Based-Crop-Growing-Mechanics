using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Gameplay
{
    public enum Layers
    {
        GROUND = 0, OBJECTS = 1, SELECTION = 100
    }
    public struct TilemapLayer
    {
        public int layer;
        public Tilemap tilemap;
    }

    public class TilemapLayerController : MonoBehaviour
    {
        [Header("Tilemap Layers")]
        public TilemapLayer GroundLayer;
        public TilemapLayer ObjectsLayer;
        public TilemapLayer SelectionLayer;

        [Header("Tilemap Layers")]
        [SerializeField]
        private Tilemap groundTilemap;
        [SerializeField]
        private Tilemap objectsTilemap;
        [SerializeField]
        private Tilemap selectionTilemap;

        [Header("UI Elements")]
        public Text selectionLayerText;


        private int currentSelectionLayer = (int)Layers.OBJECTS;

        public void Awake()
        {
            InitLayers();
        }

        private void Start()
        {
            selectionLayerText.text = currentSelectionLayer.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Home))
            {
                IncreaseSelectionLayer();
            }
            if (Input.GetKeyDown(KeyCode.End))
            {
                DecreaseSelectionLayer();
            }
        }

        private void InitLayers()
        {
            GroundLayer = new TilemapLayer
            {
                layer = (int)Layers.GROUND,
                tilemap = groundTilemap,
            };
            ObjectsLayer = new TilemapLayer
            {
                layer = (int)Layers.OBJECTS,
                tilemap = objectsTilemap,
            };
            SelectionLayer = new TilemapLayer
            {
                layer = (int)Layers.SELECTION,
                tilemap = selectionTilemap,
            };
        }


        public TilemapLayer GetCurrentSelectedLayer()
        {
            switch (currentSelectionLayer)
            {
                case (int)Layers.GROUND: return GroundLayer;
                case (int)Layers.OBJECTS: return ObjectsLayer;
                case (int)Layers.SELECTION: return SelectionLayer;
                default: return ObjectsLayer;
            }

        }

        #region Debug Tools

        public void IncreaseSelectionLayer()
        {
            currentSelectionLayer = currentSelectionLayer + 1;
            selectionLayerText.text = currentSelectionLayer.ToString();
        }
        public void DecreaseSelectionLayer()
        {
            currentSelectionLayer = currentSelectionLayer - 1;
            selectionLayerText.text = currentSelectionLayer.ToString();
        }


        #endregion
    }

}