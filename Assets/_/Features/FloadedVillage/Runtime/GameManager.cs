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

                WaterFlow();
            }

            void Update()
            {
                
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

            WaterFlow();
        }

        private void WaterFlow()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == 0)
                    {
                        var targetAround = CheckAroundForTarget(y, x, 1);
                        if (targetAround)
                        {
                            FillWaterAt(y, x);
                            WaterFlow();
                            break;
                        }
                    }
                }
            }
        }

        private void FillWaterAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = 1;
            m_environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), m_waterTile);
        }

        private bool CheckAroundForTarget(int arrY, int arrX, int target)
        {
            Debug.Log($"Check around Tile at [{arrY},{arrX}] = {_currentLevelTiles[arrY, arrX]}");
            Debug.Log("Left");
            // Check left
            if (arrX - 1 >= 0 && _currentLevelTiles[arrY, arrX - 1] == target)
            {
                Debug.Log("Found " + target + " left");
                return true;
            }
            Debug.Log("Top");
            // Check top
            if (arrY - 1 <= _currentLevelTiles.GetLength(0)
                && _currentLevelTiles[arrY - 1, arrX] == target)
            {
                Debug.Log("Found " + target + " top");
                return true;
            }
            if (arrX + 1 < _currentLevelTiles.GetLength(1) - 1) Debug.Log($"Check Right at [{arrY},{arrX + 1}] = {_currentLevelTiles[arrY, arrX + 1]}");
            // Check right
            if (arrX + 1 < _currentLevelTiles.GetLength(1) - 1 && _currentLevelTiles[arrY, arrX + 1] == target)
            {
                Debug.Log("Found " + target + " right");
                return true;
            }
            Debug.Log("Bot");
            // Check bot
            if (arrY + 1 >= 0 && _currentLevelTiles[arrY + 1, arrX] == target)
            {
                Debug.Log("Found " + target + " bot");
                return true;
            }

            return false;
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
