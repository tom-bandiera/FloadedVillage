using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace FloadedVillage.Runtime
{
    public class InputHandler : MonoBehaviour
    {
        #region Publics

        public Tilemap m_environmentTilemap;

        public UnityEvent<Vector3> m_onTileClicked;

        #endregion

        #region Unity API


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                
                Vector3 worldPosition = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(mousePosition));
                worldPosition.z = 0;

                Vector3Int cellPosition = Vector3Int.FloorToInt(m_environmentTilemap.WorldToCell(worldPosition));

                m_onTileClicked.Invoke(cellPosition);
            }
        }

        #endregion

        #region Main methods

        #endregion

        #region Utils

        #endregion

        #region Privates & Protected

        [SerializeField] GameManager _gameManager;
	
        #endregion
    }

}
