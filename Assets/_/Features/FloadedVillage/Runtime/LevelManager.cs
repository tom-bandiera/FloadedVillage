using System;
using System.Collections;
using System.Collections.Generic;
using log4net.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FloadedVillage.Runtime
{
    public class LevelManager : MonoBehaviour
    {
        #region Publics
	
        #endregion

        #region Unity API

        private void Awake()
        {
	        Time.timeScale = 1f;
	        if (SceneManager.GetActiveScene().name == "MainMenu")
	        {
		        _uiManager.ShowMainMenu();
	        }
        }
		
        #endregion

        #region Main methods
        
        public void RetryLevel()
        {
	        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	        Time.timeScale = 1f;
        }

        public void GameOver()
        {
	        _uiManager.ShowGameOverMenu();
	        Time.timeScale = 0f;
        }
        
        public void LevelCompleted()
        {
	        _uiManager.ShowLevelCompletedMenu();
	        Time.timeScale = 0f;
        }
        
        public void NextLevel()
        {
	        SceneManager.LoadScene(_nextLevel.name);
	        Time.timeScale = 1f;
        }
        
        public void Quit()
        {
	        Application.Quit();
        }
	
        #endregion

        #region Utils
	
        #endregion

        [SerializeField] private SceneAsset _nextLevel;
        [SerializeField] private UIManager _uiManager;

        #region Privates & Protected

        #endregion
    }

}
