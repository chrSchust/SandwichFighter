using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {
	public Button buttonNewGame;
	public Button buttonLevelSelection;
	public Button buttonExit;
	public Button buttonLevel1;
	public Button buttonLevel2;
	public Button buttonLevel3;
    public Button back;
    List<Button> levelButtons;
    public GameObject panelMainMenu;
	public GameObject panelLevelSelection;

	private int unlockedLevels;

	// Use this for initialization
	void Start () {
        // Check if player already unlocked some levels
        // Is -1 if the player hasn't unlocked any level.
        // So hide the levelSelection button

        //only for testing!
        PlayerPrefs.DeleteAll();

        levelButtons = new List<Button> { buttonLevel1, buttonLevel2, buttonLevel3 };
		unlockedLevels = PlayerPrefs.GetInt(SceneKeys.PLAYER_PREF_KEY_UNLOCKED_LEVELS, -1);
        //unlockedLevels = 3;

        if (unlockedLevels > 0) {
			// Show levelSelection button
            for(int i = 0; i < unlockedLevels; i++)
            {
                levelButtons[i].gameObject.SetActive(true);
            }
		} else {
			// Hide levelSelection button
			buttonLevelSelection.gameObject.SetActive(false);
		}
		AddListenersToButtons ();
	}

	private void AddListenersToButtons() {
		buttonNewGame.onClick.AddListener(() => OnButtonNewGameClicked());
        buttonLevelSelection.onClick.AddListener(() => OnButtonLevelSelectionClicked());
        back.onClick.AddListener(() => OnButtonBackClicked());
        buttonExit.onClick.AddListener(() => OnButtonExitClicked());
        foreach (Button b in levelButtons)
        {
            Button tempButton = b;
            b.onClick.AddListener(() => OnLevelButtonClicked(tempButton));
        }
    }

    private void OnButtonExitClicked()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    private void OnButtonBackClicked()
    {
        buttonLevelSelection.gameObject.SetActive(true);
        buttonNewGame.gameObject.SetActive(true);
        buttonExit.gameObject.SetActive(true);
        panelLevelSelection.SetActive(false);
    }

    private void OnButtonLevelSelectionClicked()
    {
        buttonLevelSelection.gameObject.SetActive(false);
        buttonNewGame.gameObject.SetActive(false);
        buttonExit.gameObject.SetActive(false);
        panelLevelSelection.SetActive(true);
    }

    private void OnLevelButtonClicked(Button buttonLevel)
    {
        switch (levelButtons.IndexOf(buttonLevel))
        {
            case 0:
                LoadScene("Restaurant_Level1");
                break;
            case 1:
                LoadScene("Restaurant_Level2");
                break;
            case 2:
                LoadScene("Restaurant_Level3");
                break;
        }
    }

    private void OnButtonNewGameClicked() {
		LoadScene ("Restaurant_Level1");
	}

	private void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}
}
