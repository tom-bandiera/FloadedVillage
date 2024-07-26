using System;
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
        public Tile m_cropsTile;
        
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
            var arrayCoordinates = _gridGenerator.GetArrayCoordinatesFromPosition(tilemapCoordinates);

            if (_currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] != EnumTile.TYPE.SAND) return;
            
            m_environmentTilemap.SetTile(tilemapCoordinates, m_emptyTile);
            _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] = EnumTile.TYPE.EMPTY;

            WaterFlow();
            GrowSeeds();
        }

        private void WaterFlow()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == EnumTile.TYPE.EMPTY)
                    {
                        bool isWaterAround = CheckAroundForTarget(y, x, EnumTile.TYPE.WATER);
                        if (isWaterAround)
                        {
                            FillWaterAt(y, x);
                            WaterFlow();
                            break;
                        }
                    }
                }
            }
        }
        
        private void GrowSeeds()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == EnumTile.TYPE.SEEDS)
                    {
                        bool isWaterAround = CheckAroundForTarget(y, x, EnumTile.TYPE.WATER);
                        if (isWaterAround)
                        {
                            GrowSeedAt(y, x);
                        }
                    }
                }
            }
        }

        private void FillWaterAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.WATER;
            m_environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), m_waterTile);
        }

        private void GrowSeedAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.CROPS;
            m_environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), m_cropsTile);
        }

        private bool CheckAroundForTarget(int arrY, int arrX, EnumTile.TYPE target)
        {
            Debug.Log($"Check around Tile at [{arrY},{arrX}] = {_currentLevelTiles[arrY, arrX]}");
            
            // Check left
            if (arrX - 1 >= 0 && _currentLevelTiles[arrY, arrX - 1] == target)
            {
                Debug.Log("Found " + target + " left");
                return true;
            }
           
            // Check top
            if (arrY - 1 <= _currentLevelTiles.GetLength(0)
                && _currentLevelTiles[arrY - 1, arrX] == target)
            {
                Debug.Log("Found " + target + " top");
                return true;
            }
            
            // Check right
            if (arrX + 1 < _currentLevelTiles.GetLength(1) - 1 && _currentLevelTiles[arrY, arrX + 1] == target)
            {
                Debug.Log("Found " + target + " right");
                return true;
            }
            Debug.Log($"here: [{arrY + 1},{arrX}] ");
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

        private EnumTile.TYPE[,] _currentLevelTiles;
        [SerializeField] private GridGenerator _gridGenerator;

        #endregion
    }
}
