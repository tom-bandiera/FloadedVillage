using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridManager.Runtime;
using UnityEngine.Tilemaps;

namespace FloadedVillage.Runtime
{
    public class GameManager : MonoBehaviour
    {
        #region Publics

        public Tile m_emptyTile;
        public Tile m_waterTile;
        
        public Tilemap m_environmentTilemap;
	
        #endregion

        #region Unity API
        
    		void Start()
    		{
                _currentLevelTiles = _gridGenerator.m_initialLevelTiles;

                for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
                {
                    for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                    {
                        Debug.Log($"Tile at ({x},{y}) = {_currentLevelTiles[y, x]}");
                    }
                }
            }
            
        #endregion

        #region Main methods

        public void OnTileClicked(Vector3 vector)
        {
            Vector3Int tilemapCoordinates = new Vector3Int((int)vector.x, (int)vector.y, 0);
            
            DigAt(tilemapCoordinates);
        }
        
        private void DigAt(Vector3Int tilemapCoordinates)
        {
            Vector3Int arrayCoordinates = _gridGenerator.GetArrayCoordinatesFromPosition(tilemapCoordinates);

            if (_currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] != 2) return;
            
            m_environmentTilemap.SetTile(tilemapCoordinates, m_emptyTile);
            Debug.Log("Tile changed at: " + arrayCoordinates);
            _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] = 0;
            Debug.Log("Tile in array is now :" + _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x]);

            CheckAroundForWater(arrayCoordinates);
        }

        private void WaterFlow()
        {
            
        }

        private void FillWaterAt(Vector3Int arrayCoordinates)
        {
            _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] = 1;
            m_environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrayCoordinates.x, arrayCoordinates.y, 0)), m_waterTile);
        }

        private void CheckAroundForWater(Vector3Int arrayCoordinates)
        {
            Debug.Log("ArrX: " + arrayCoordinates.x + "ArrY: " + arrayCoordinates.y);
            // Check left
            if (arrayCoordinates.x - 1 >= 0 && _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x - 1] == 1)
            {
                FillWaterAt(arrayCoordinates);
                //CheckAroundForWater(new Vector3Int(arrayCoordinates.x - 1, arrayCoordinates.y, 0));
            }
            // Check top
            if (arrayCoordinates.y - 1 <= _currentLevelTiles.GetLength(0)
                && _currentLevelTiles[arrayCoordinates.y - 1, arrayCoordinates.x] == 1)
            {
                FillWaterAt(arrayCoordinates);
                //CheckAroundForWater(new Vector3Int(arrayCoordinates.y - 1, arrayCoordinates.x, 0));
            }
            // Check right
            if (arrayCoordinates.x + 1 >= 0 && _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x + 1] == 1)
            {
                FillWaterAt(arrayCoordinates);
                //CheckAroundForWater(new Vector3Int(arrayCoordinates.x + 1, arrayCoordinates.y, 0));
            }
            // Check bot
            if (arrayCoordinates.y + 1 >= 0 && _currentLevelTiles[arrayCoordinates.y + 1, arrayCoordinates.x] == 1)
            {
                FillWaterAt(arrayCoordinates);
                //CheckAroundForWater(new Vector3Int(arrayCoordinates.x, arrayCoordinates.y + 1, 0));
            }
        }

        #endregion

        #region Utils

        #endregion

        #region Privates & Protected

        private int[,] _currentLevelTiles;
        [SerializeField] private GridGenerator _gridGenerator;

        #endregion
    }
}
