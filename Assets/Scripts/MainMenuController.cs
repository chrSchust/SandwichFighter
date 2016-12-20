using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	public Button buttonNewGame;
	public Button buttonLevelSelection;
	public Button buttonExit;
	public Button buttonLevel1;
	public Button buttonLevel2;
	public Button buttonLevel3;
	public GameObject panelMainMenu;
	public GameObject panelLevelSelection;

	private int unlockedLevels;

	// Use this for initialization
	void Start () {
		// Check if player already unlocked some levels
		// Is -1 if the player hasn't unlocked any level.
		// So hide the levelSelection button
		// TODO create KEY Class for the key "unlockedLevels"
		unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", -1);
		if (unlockedLevels > 0) {
			// Show levelSelection button
		} else {
			// Hide levelSelection button
			buttonLevelSelection.gameObject.SetActive(false);
		}
	}

	private void AddListenersToButtons() {
		buttonNewGame.onClick.AddListener(() => OnButtonNewGameClicked());
	}

	private void OnButtonNewGameClicked() {
		LoadScene ("Restaurant");
	}

	private void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}
}
