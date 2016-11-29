using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour {
	public static int VISIBLE = 1;
	public static int INVISIBLE = 0;
	public const string PANEL_BACKGROUND = "PanelBackground";
	public const string PANEL_LEVEL_SELECTION = "PanelLevelSelection";
	public const string PANEL_SELECTED_SANDWICH = "PanelSelectedSandwich";
	public const string PANEL_SANDWICH = "PanelSandwich";

	public GameObject prefabButtonLevel;
	public GameObject panelBackground;
	public GameObject panelLevelSelection;
	public GameObject panelSelectedSandwich;
	public GameObject panelSandwich;

	private List<Level> levels;
	private GameFlowController gameFlowController;

	// Use this for initialization
	void Start () {
		gameFlowController = GameObject.FindGameObjectWithTag ("GameFlowController").GetComponent<GameFlowController> ();
		if (gameFlowController == null) {
			Debug.LogError ("GameFlowController not found");
		}

		// Set visibilies for the first time
		SetVisibilityPanel (PANEL_BACKGROUND, true);
		SetVisibilityPanel (PANEL_SELECTED_SANDWICH, false);
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, false);
		SetVisibilityPanel (PANEL_SANDWICH, false);

		// Check if a level is already unlocked
		if(gameFlowController.GetUnlockedLevelsCount() > 0) {
			// Show selection of levels
			SetVisibilityPanel(PANEL_LEVEL_SELECTION, true);
			AddButtonToLevelSelection (gameFlowController.GetUnlockedLevelsCount());
		}
	}

	public void SetLevels(List<Level> levels) {
		this.levels = levels;
	}

	private void AddButtonToLevelSelection(int unlockedLevelsCount) {
		GameObject panel = GetPanel (PANEL_LEVEL_SELECTION);
		RectTransform panelRect = panel.GetComponentInChildren<RectTransform> ();
		foreach (Transform child in panel.transform) {
            GameObject.Destroy(child.gameObject);
        }

		for(int i = 0; i < unlockedLevelsCount; i++) {
			GameObject goButton = Instantiate(prefabButtonLevel) as GameObject;
			goButton.transform.SetParent(panelRect, false);
            goButton.GetComponentInChildren<Text>().text = "Level " + (i + 1);
            goButton.GetComponentInChildren<Text>().fontSize = 16;
//            goButton.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(index));
		
        }
	}

	private void SetVisibilityPanel(string panelKey, bool visibility) {
		GameObject panel = GetPanel (panelKey);
		panel.SetActive (visibility);
	}

	private GameObject GetPanel(string panelKey) {
		switch (panelKey) {
			case PANEL_BACKGROUND:
				return panelBackground;
			case PANEL_LEVEL_SELECTION:
				return panelLevelSelection;
			case PANEL_SANDWICH:
				return panelSandwich;
			case PANEL_SELECTED_SANDWICH:
				return panelSelectedSandwich;
			default :
				return null;
		}
	}
}
