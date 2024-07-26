using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloadedVillage.Runtime
{
    public class UIManager : MonoBehaviour
    {
        #region Publics
	
        #endregion

        #region Unity API
		
    	    // Start is called before the first frame update
    		void Start()
    		{
			
    		}

    		// Update is called once per frame
    		void Update()
    		{
			
    		}
		
        #endregion

        #region Main methods

        public void ShowMainMenu()
        {
	        _mainMenu.SetActive(true);
        }

        public void ShowLevelCompletedMenu()
        {
	        _levelCompletedMenu.SetActive(true);
        }
        
        public void HideLevelCompletedMenu()
        {
	        _levelCompletedMenu.SetActive(false);
        }
        
        
        public void ShowGameOverMenu()
        {
	        _gameOverMenu.SetActive(true);
        }
        
        public void HideGameOverMenu()
        {
	        _gameOverMenu.SetActive(false);
        }
	
        #endregion

        #region Utils
	
        #endregion

        #region Privates & Protected

        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _levelCompletedMenu;
        [SerializeField] private GameObject _gameOverMenu;

        #endregion
    }

}
