using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
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

    public void UpdateCurrentActions(int currentActions, int maxActions)
    {
	    _currentActions.text = "Actions left: " + currentActions + " / " + maxActions;
    }
	
    #endregion

    #region Utils
	
    #endregion

    #region Privates & Protected

    [SerializeField] private TMP_Text _currentActions;

    #endregion
}

