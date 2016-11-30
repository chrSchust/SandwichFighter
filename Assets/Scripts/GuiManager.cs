﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GuiManager : MonoBehaviour {
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
	private int sandwichChose;
	private int chosenLevel;
	private Transform panelBread;
	private Transform panelIngredient1;
	private Transform panelIngredient2;
	private Dropdown dropdownBread;
	private Dropdown dropdownIngredient1;
	private Dropdown dropdownIngredient2;


	// Use this for initialization
	void Start () {
		
	}

	public void Init(List<Level> levels) {
		sandwichChose = 0;
		gameFlowController = GameObject.FindGameObjectWithTag ("GameFlowController").GetComponent<GameFlowController> ();
		if (gameFlowController == null) {
			Debug.LogError ("GameFlowController not found");
		}
		SetLevels (levels);
		FindAndSetAllSubPanels ();

		// Set visibilies for the first time
		SetVisibilityPanel (PANEL_BACKGROUND, true);
		SetVisibilityPanel (PANEL_SELECTED_SANDWICH, false);
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, false);
		SetVisibilityPanel (PANEL_SANDWICH, false);
		SetVisibilityCursor (true);
		// Check if a level is already unlocked
		if (gameFlowController.GetUnlockedLevelsCount () > 0) {
			// Show selection of levels
			SetVisibilityPanel (PANEL_LEVEL_SELECTION, true);
			AddButtonToLevelSelection (gameFlowController.GetUnlockedLevelsCount ());
		} else {
			ShowSandwichCombinator (this.sandwichChose);
			LevelButtonClicked (1);
		}
	}

	public void SetLevels(List<Level> levels) {
		this.levels = levels;
	}

	private void SetChosenLevel(int chosenLevel){
		this.chosenLevel = chosenLevel;
	}

	private int GetChosenLevel(){
		return this.chosenLevel;
	}

	private void ShowSandwichCombinator(int sandwichChosen) {
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, false);
		SetVisibilityPanel (PANEL_SANDWICH, true);
		SetVisibilityPanel (PANEL_BACKGROUND, true);
		if (sandwichChosen == 0) {
			// No sandwich was chosen
			SetVisibilityPanel(PANEL_SELECTED_SANDWICH, false);
			SetVisibilityPreviousSandwichButton (false);
		} else {
			// one or more sandwiches are already available
			// so show PANEL_SELECTED_SANDWICH
			SetVisibilityPanel(PANEL_SELECTED_SANDWICH, true);
			SetVisibilityPreviousSandwichButton (true);
		}

		// First show bread panel after that the other
		// Ingredients
		SetVisibilityNextSandwichButton (false);
		ShowBreadPanel ();
	}

	private void ShowBreadPanel() {
//		DisableAllSandwichSubPanels ();
	}

	private void InitializeAllDropdownsWithValues(int chosenLevel) {
		// Get all dropdowns
		FindAndSetAllDropdowns();

		SetLevels (gameFlowController.GetLevels ());
		Level level = this.levels[chosenLevel-1];

		// TODO Get and set all available breads

		// Get and set all available ingredients
		List<string> optionsTmp = new List<string>();
		List<Ingredient> ingredients = level.availableIngredients;
		foreach (Ingredient ingredient in ingredients) {
			optionsTmp.Add (ingredient.getName());
		}
		dropdownBread.ClearOptions ();

		// TODO add options
		dropdownIngredient1.ClearOptions();
		dropdownIngredient1.AddOptions (optionsTmp);
		dropdownIngredient2.ClearOptions ();
		dropdownIngredient2.AddOptions (optionsTmp);
		dropdownIngredient2.value = -1;
	}

	private void DisableAllSandwichSubPanels() { 
		panelBread.gameObject.SetActive (false);
		panelIngredient1.gameObject.SetActive(false);
		panelIngredient2.gameObject.SetActive(false);
	}

	private void SetVisibilityPreviousSandwichButton(bool visible) {
		Transform previousButton = panelSandwich.transform.FindChild ("ButtonPreviousSandwich");
		if (previousButton == null) {
			Debug.LogError ("Previous Button is null");
		}
		previousButton.gameObject.SetActive (visible);
	}

	private void SetVisibilityNextSandwichButton(bool visible) {
		Transform previousButton = panelSandwich.transform.FindChild ("ButtonNextSandwich");
		if (previousButton == null) {
			Debug.LogError ("Previous Button is null");
		}
		previousButton.gameObject.SetActive (visible);
	}

	private void SetVisibilityCursor(bool visible) {
		GameObject player = GameObject.Find("FirstPersonCharacter");
		player.GetComponent<Crosshair> ().enabled = !visible;
		player.GetComponent<FirstPersonController> ().enabled = !visible;
		player.GetComponentInChildren<WeaponController> ().enabled = !visible;
		Cursor.visible = visible;
		Cursor.lockState = CursorLockMode.None;
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
			goButton.GetComponent<Button>().onClick.AddListener(() => LevelButtonClicked(i + 1));
        }
	}

	private void LevelButtonClicked(int chosenLevel) {
		SetChosenLevel (chosenLevel);
		ShowSandwichCombinator (0);
		InitializeAllDropdownsWithValues (chosenLevel);
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

	private void FindAndSetAllSubPanels() {
		// Get the sub panels
		Transform panelSandwichTransform = panelSandwich.transform;
		panelBread = panelSandwichTransform.FindChild("PanelBread");
		Transform panelIngredient1Transform = panelSandwich.transform;
		panelIngredient1 = panelIngredient1Transform.FindChild("PanelIngredient1");
		Transform panelIngredient2Transform = panelSandwich.transform;
		panelIngredient2 = panelIngredient2Transform.FindChild("PanelIngredient2");
		if (panelBread == null ||
			panelIngredient1 == null ||
			panelIngredient2 == null) {
			Debug.LogError ("A sub panel is null");
		}
	}

	private void FindAndSetAllDropdowns() {
		string dropdownText = "Dropdown";
		dropdownBread = panelBread.FindChild (dropdownText).GetComponent<Dropdown>();
		dropdownIngredient1= panelIngredient1.FindChild (dropdownText).GetComponent<Dropdown>();
		dropdownIngredient2 = panelIngredient2.FindChild (dropdownText).GetComponent<Dropdown>();
		if (dropdownBread == null ||
		   dropdownIngredient1 == null ||
		   dropdownIngredient2 == null) {
			Debug.LogError ("A dropdown is null");
		}
	}
}
