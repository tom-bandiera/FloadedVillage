using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace GridManager.Runtime
{
    public class GridGenerator : MonoBehaviour
    {
        #region Publics
        
        public Tilemap m_environmentTilemap;
        public EnumTile.TYPE[,] m_initialLevelTiles;

        #endregion

        #region Unity API
        
        void Awake()
        {
            _bounds = m_environmentTilemap.cellBounds;
            ConvertTilemapToArray();
        }

        #endregion

        #region Main methods

        private void ConvertTilemapToArray()
        {
            int numRows = _bounds.size.y;
            int numCols = _bounds.size.x;
            m_initialLevelTiles = new EnumTile.TYPE[numRows, numCols];

            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numCols; x++)
                {
                    Vector3Int cellPosition = GetPositionFromArrayCoordinates(new Vector3Int(x, y, 0));

                    TileBase tilemapTile = m_environmentTilemap.GetTile(cellPosition);

                    EnumTile.TYPE tileValue = GetTileValue(tilemapTile.name);

                    m_initialLevelTiles[y, x] = tileValue;
                }
            }
            
            PrintArray();
        }

        #endregion

        #region Utils

        public Vector3Int GetArrayCoordinatesFromPosition(Vector3 vector)
        {
            Vector3Int position = new Vector3Int((int) vector.x, (int) vector.y, (int) vector.z);
            BoundsInt bounds = m_environmentTilemap.cellBounds;

            return new Vector3Int(position.x - _bounds.xMin, Mathf.Abs(position.y - (bounds.yMax - 1)));
        }
        
        public Vector3Int GetPositionFromArrayCoordinates(Vector3Int coordinates)
        {
            return new Vector3Int(coordinates.x + _bounds.xMin, _bounds.yMax - (coordinates.y + 1), 0);
        }

        private void PrintArray()
        {
            for (int y = 0; y < m_initialLevelTiles.GetLength(0); y++)
            {
                for (int x = 0; x < m_initialLevelTiles.GetLength(1); x++)
                {
                    Debug.Log($"Tile at [{y},{x}] = {m_initialLevelTiles[y, x]}");
                }
            }
        }

        private EnumTile.TYPE GetTileValue(string tileName)
        {
            switch (tileName)
            {
                case "empty":
                    return EnumTile.TYPE.EMPTY;
                    
                case "water":
                    return EnumTile.TYPE.WATER;

                case "sand":
                    return EnumTile.TYPE.SAND;

                case "seeds":
                    return EnumTile.TYPE.SEEDS;

                case "crops":
                    return EnumTile.TYPE.CROPS;
                
                case "bridge_merged":
                    return EnumTile.TYPE.BRIDGE;
                
                case "villager_merged":
                    return EnumTile.TYPE.VILLAGER;
                
                case "villager_drown_merged":
                    return EnumTile.TYPE.VILLAGER_DROWNED;
                
                case "zombie_merged":
                    return EnumTile.TYPE.ZOMBIE;
                
                case "zombie_drown_merged":
                    return EnumTile.TYPE.ZOMBIE_DROWNED;

                default:
                    return EnumTile.TYPE.NONE;

            }
        }

        #endregion

        #region Privates & Protected

        private BoundsInt _bounds;

        #endregion
    }

}
