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
        public int[,] m_initialLevelTiles;

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
            m_initialLevelTiles = new int[numRows, numCols];

            for (int y = 0; y < numRows; y++)
            {
                for (int x = 0; x < numCols; x++)
                {
                    Vector3Int cellPosition = GetPositionFromArrayCoordinates(new Vector3Int(x, y, 0));

                    TileBase tilemapTile = m_environmentTilemap.GetTile(cellPosition);

                    int tileValue = GetTileValue(tilemapTile);

                    m_initialLevelTiles[y, x] = tileValue;
                }
            }
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
                    Debug.Log($"Tile at ({x},{y}) = {m_initialLevelTiles[y, x]}");
                }
            }
        }

        private int GetTileValue(TileBase tile)
        {
            switch (tile.name)
            {
                case "empty":
                    return 0;
                    
                case "water":
                    return 1;

                case "sand":
                    return 2;

                case "seeds":
                    return 3;

                case "crops":
                    return 4;

                default:
                    return -1;

            }
        }

        #endregion

        #region Privates & Protected

        private BoundsInt _bounds;

        #endregion
    }

}
