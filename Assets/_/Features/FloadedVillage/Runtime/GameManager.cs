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

        #endregion

        #region Unity API
        
    		void Start()
    		{
                _currentLevelTiles = _gridGenerator.m_initialLevelTiles;
                _zombiesLeftCounter = _gridGenerator.m_zombiesCounter;
                _seedsLeftCounter = _gridGenerator.m_seedsCounter;
                _hudManager.UpdateCurrentActions(_actionsCounter, _maxActions);

                WaterFlow();
            }

            void Update()
            {
                
            }
            
        #endregion

        #region Main methods

        public void OnTileClicked(Vector3 vector)
        {
            if (_isGameOver) return;
            
            Vector3Int tilemapCoordinates = new Vector3Int((int)vector.x, (int)vector.y, 0);
            DigAt(tilemapCoordinates);
        }

        private void NewAction()
        {
            _actionsCounter++;
            _hudManager.UpdateCurrentActions(_actionsCounter, _maxActions);
        }
        
        private void DigAt(Vector3Int tilemapCoordinates)
        {
            var arrayCoordinates = _gridGenerator.GetArrayCoordinatesFromPosition(tilemapCoordinates);

            if (_currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] != EnumTile.TYPE.SAND) return;

            NewAction();
            _environmentTilemap.SetTile(tilemapCoordinates, _emptyTile);
            _currentLevelTiles[arrayCoordinates.y, arrayCoordinates.x] = EnumTile.TYPE.EMPTY;

            WaterFlow();
            
            DrownVillagers();
            DrownZombies();
            GrowSeeds();
            
            if (!_isGameOver)
            {
                if (!_isLevelComplete && _seedsLeftCounter == 0 && _zombiesLeftCounter == 0)
                {
                    _isLevelComplete = true;
                    // Invoke Level Complete after 1 sec
                    _levelManager.LevelCompleted();
                    _actionsCounter = 0;
                } else if (_actionsCounter >= _maxActions)
                {
                    _isGameOver = true;
                    _levelManager.GameOver();
                }
            }
        }

        private void WaterFlow()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == EnumTile.TYPE.EMPTY || _currentLevelTiles[y, x] == EnumTile.TYPE.BRIDGE)
                    {
                        if (IsWaterAround(y, x))
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
                        if (IsWaterAround(y, x))
                        {
                            GrowSeedAt(y, x);
                        }
                    }
                }
            }
        }

        private void DrownZombies()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == EnumTile.TYPE.ZOMBIE)
                    {
                        if (IsWaterAround(y, x))
                        {
                            DrownZombieAt(y, x);
                            WaterFlow();
                            break;
                        }
                    }
                }
            }
        }
        
        private void DrownVillagers()
        {
            for (int y = 0; y < _currentLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < _currentLevelTiles.GetLength(1); x++)
                {
                    if (_currentLevelTiles[y, x] == EnumTile.TYPE.VILLAGER)
                    {
                        if (IsWaterAround(y, x))
                        {
                            DrownVillagerAt(y, x);
                        }
                    }
                }
            }
        }

        private void FillWaterAt(int arrY, int arrX)
        {
            if (_currentLevelTiles[arrY, arrX] == EnumTile.TYPE.EMPTY)
            {
                _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.WATER;
                _environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), _waterTile);
            }
            else
            {
                _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.BRIDGE_WATER;
                _environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), _bridgeWaterTile);
            }
        }

        private void GrowSeedAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.CROPS;
            _environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), _cropsTile);
            _seedsLeftCounter--;
        }

        private void DrownZombieAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.ZOMBIE_DROWNED;
            _environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), _zombieDrownedTile);
            _zombiesLeftCounter--;
        }
        
        private void DrownVillagerAt(int arrY, int arrX)
        {
            _currentLevelTiles[arrY, arrX] = EnumTile.TYPE.VILLAGER_DROWNED;
            _environmentTilemap.SetTile(_gridGenerator.GetPositionFromArrayCoordinates(new Vector3Int(arrX, arrY)), _villagerDrownedTile);
            _isGameOver = true;
        }

        private bool CheckAroundForTarget(int arrY, int arrX, EnumTile.TYPE target)
        {
            // Check left
            if (arrX - 1 >= 0 && _currentLevelTiles[arrY, arrX - 1] == target)
            {
                return true;
            }
           
            // Check top
            if (arrY - 1 >= 0
                && _currentLevelTiles[arrY - 1, arrX] == target)
            {
                return true;
            }
            
            // Check right
            if (arrX + 1 <= _currentLevelTiles.GetLength(1) - 1 && _currentLevelTiles[arrY, arrX + 1] == target)
            {
                return true;
            }

            // Check bot
            if (arrY + 1 <= _currentLevelTiles.GetLength(0) - 1 && _currentLevelTiles[arrY + 1, arrX] == target)
            {
                return true;
            }
            
            return false;
        }

        private bool IsWaterAround(int y, int x)
        {
            return
                CheckAroundForTarget(y, x, EnumTile.TYPE.WATER) 
                || CheckAroundForTarget(y, x, EnumTile.TYPE.BRIDGE_WATER)
                || CheckAroundForTarget(y, x, EnumTile.TYPE.VILLAGER_DROWNED)
                || CheckAroundForTarget(y, x, EnumTile.TYPE.ZOMBIE_DROWNED);
        }

        #endregion

        #region Utils

        #endregion

        #region Privates & Protected
        
        private EnumTile.TYPE[,] _currentLevelTiles;
        private bool _isGameOver;
        private bool _isLevelComplete;
        private int _seedsLeftCounter;
        private int _zombiesLeftCounter;
        private int _actionsCounter;
        [SerializeField] private int _maxActions;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private HUDManager _hudManager;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private Tile _emptyTile;
        [SerializeField] private Tile _waterTile;
        [SerializeField] private Tile _cropsTile;
        [SerializeField] private Tile _bridgeWaterTile;
        [SerializeField] private Tile _zombieDrownedTile;
        [SerializeField] private Tile _villagerDrownedTile;
        [SerializeField] private Tilemap _environmentTilemap;

        #endregion
    }
}
