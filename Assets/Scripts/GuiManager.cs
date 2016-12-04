using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GuiManager : MonoBehaviour {
	public const string PANEL_BACKGROUND = "PanelBackground";
	public const string PANEL_LEVEL_SELECTION = "PanelLevelSelection";
	public const string PANEL_SELECTED_SANDWICH = "PanelSelectedSandwich";
	public const string PANEL_SANDWICH = "PanelSandwich";
	public const string BUTTON_NEXT_SANWICH = "ButtonNextSandwich";

	public GameObject prefabButtonLevel;
	public GameObject panelBackground;
	public GameObject panelLevelSelection;
	public GameObject panelSelectedSandwich;
	public GameObject panelSandwich;

	private List<Level> levels;
	private GameFlowController gameFlowController;
	private Transform panelBread;
	private Transform panelIngredient1;
	private Transform panelIngredient2;
	private Dropdown dropdownBread;
	private Dropdown dropdownIngredient1;
	private Dropdown dropdownIngredient2;
	// TODO Add bread

	private Ingredient ingredient1;
	private Ingredient ingredient2;
	private Level activeLevel;

	void Start() {

	}

	public void Init(GameFlowController gameFlowController, List<Level> levels, int unlockedLevelsCount) {
		this.gameFlowController = gameFlowController;
		SetLevels (levels);
		FindAndSetAllSubPanels ();

		// Set visibilities for the first time
		SetVisibilityPanel (PANEL_BACKGROUND, true);
		SetVisibilityPanel (PANEL_SELECTED_SANDWICH, false);
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, false);
		SetVisibilityPanel (PANEL_SANDWICH, false);
		SetVisibilityCursor (true);
		// Check if a level is already unlocked
		if (unlockedLevelsCount > 0) {
			// Show selection of levels
			SetVisibilityPanel (PANEL_LEVEL_SELECTION, true);
			AddButtonToLevelSelection (unlockedLevelsCount);
		} else {
			// If no level got unlocked start with the first level
			LevelButtonClicked (0);
		}
	}

	public void SetLevels(List<Level> levels) {
		this.levels = levels;
	}

	private void ShowSandwichCombinator(int sandwichChosen, int chosenLevel) {
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, false);
		InitializeAllDropdownsWithValues (chosenLevel);

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

		SetVisibilityPanel (PANEL_SANDWICH, true);
		SetVisibilityNextSandwichButton (true);
		SetDropdownBreadListener (dropdownBread, chosenLevel);
		SetDropdownIngredient1Listener (dropdownIngredient1, chosenLevel);
		SetDropdownIngredient2Listener (dropdownIngredient2, chosenLevel);
	}

	private void SetDropdownBreadListener(Dropdown dropdownBread, int chosenLevel) {
		if (dropdownBread == null) {
			Debug.LogError ("dropdown bread is null");
		}
		// Set an empty start up value
		SetBlankDefaultDropdownValue (dropdownBread);
		// Set onChange listener
		dropdownBread.onValueChanged.AddListener (delegate {
			SetBreadValue (dropdownBread.value, chosenLevel);
		});
	}

	private void SetDropdownIngredient1Listener(Dropdown dropdownIngredient1, int chosenLevel) {
		if (dropdownIngredient1 == null) {
			Debug.LogError ("dropdown ingredient1 is null");
		}

		// Set an empty start up value
		SetBlankDefaultDropdownValue (dropdownIngredient1);
		// Set onChange listener
		dropdownIngredient1.onValueChanged.AddListener (delegate {
			SetIngredient1Value (dropdownIngredient1.value, chosenLevel);
		});
	}

	private void SetDropdownIngredient2Listener(Dropdown dropdownIngredient2, int chosenLevel) {
		if (dropdownIngredient2 == null) {
			Debug.LogError ("dropdown ingredient1 is null");
		}

		// Set an empty start up value
		SetBlankDefaultDropdownValue (dropdownIngredient2);
		// Set onChange listener
		dropdownIngredient2.onValueChanged.AddListener (delegate {
			SetIngredient2Value (dropdownIngredient2.value, chosenLevel);
		});
	}

	private void SetBreadValue(int chosenItem, int chosenLevel) {
		// TODO Implement after logic implementation

	}

	private void SetIngredient1Value(int chosenItem, int chosenLevel) {
		Level level = this.levels [chosenLevel];
		List<Ingredient> availableIngredients = level.availableIngredients;
		ingredient1 = availableIngredients [chosenItem];
	}

	private void SetIngredient2Value(int chosenItem, int chosenLevel) {
		Level level = this.levels [chosenLevel];
		List<Ingredient> availableIngredients = level.availableIngredients;
		ingredient2 = availableIngredients [chosenItem];
	}

	private void SetBlankDefaultDropdownValue(Dropdown dropdown) {
		//It is a hacky solution but I didn't find an other one
		// Add a blank dropdown option you will then remove at the end of the options list
		dropdown.GetComponent<Dropdown> ().options.Add (new Dropdown.OptionData() {text=""});
		// Select it
		dropdown.GetComponent<Dropdown>().value = dropdown.GetComponent<Dropdown>().options.Count - 1;
		// Remove it
		dropdown.GetComponent<Dropdown>().options.RemoveAt(dropdown.GetComponent<Dropdown>().options.Count - 1);
	}

	private void InitializeAllDropdownsWithValues(int chosenLevel) {
		// Get all dropdowns
		FindAndSetAllDropdowns();
		Level level = this.levels[chosenLevel];

		// TODO Get and set all available breads
		// List<string> breads is just an example
		List<string> breads = new List<string>();
		breads.Add ("Weißbrot");
		breads.Add ("Vollkornbrot");

		// Get and set all available ingredients
		List<string> optionsTmp = new List<string>();
		List<Ingredient> ingredients = level.availableIngredients;
		foreach (Ingredient ingredient in ingredients) {
			optionsTmp.Add (ingredient.getName());
		}
		dropdownBread.ClearOptions ();
		// Just an example case
		dropdownBread.AddOptions (breads);

		// Add options
		dropdownIngredient1.ClearOptions();
		dropdownIngredient1.AddOptions (optionsTmp);
		dropdownIngredient2.ClearOptions ();
		dropdownIngredient2.AddOptions (optionsTmp);
	}

//	private void DisableAllSandwichSubPanels() { 
//		panelBread.gameObject.SetActive (false);
//		panelIngredient1.gameObject.SetActive(false);
//		panelIngredient2.gameObject.SetActive(false);
//	}

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

		foreach (Level level in levels)
        {
            int index = levels.IndexOf(level);
			if (index < unlockedLevelsCount) {
				GameObject goButton = Instantiate (prefabButtonLevel) as GameObject;
				goButton.transform.SetParent (panelRect, false);
				goButton.GetComponentInChildren<Text> ().text = "Level " + (index + 1);
				goButton.GetComponentInChildren<Text> ().fontSize = 16;
				goButton.GetComponent<Button> ().onClick.AddListener (() => LevelButtonClicked (index));
			} else {
				break;
			}
        }
	}

	private void LevelButtonClicked(int chosenLevel) {
		SetActiveLevel (chosenLevel);
		ShowSandwichCombinator (0, chosenLevel);
	}

	private void SetActiveLevel(int chosenLevel) {
		this.activeLevel = levels [chosenLevel];
	}

	private void SetVisibilityPanel(string panelKey, bool visibility) {
        GameObject panel = GetPanel (panelKey);
		panel.SetActive (visibility);
		if (panelKey.Equals (PANEL_SANDWICH) && visibility == true) {
            // Set next button listener
            Transform panelTrans = panel.transform;
			Button next = panelTrans.FindChild (BUTTON_NEXT_SANWICH).GetComponent<Button> ();
			next.onClick.AddListener (() => NextSandwichButtonClicked ());
		}
	}

	private void NextSandwichButtonClicked() {
        if (ingredient1 != null && ingredient2 != null && activeLevel != null) {
			List<Ingredient> activeIngredients = new List<Ingredient> ();
			activeIngredients.Add (ingredient1);
			activeIngredients.Add (ingredient2);
			// For now just start the game
			StartLevel(activeIngredients, activeLevel);
		} else {
			Debug.Log ("wait");
		}
	}

	private void StartLevel(List<Ingredient> activeIngredients, Level activeLevel) {
		SetVisibilityCursor (false);
		SetVisibilityAllPanels(false);
		gameFlowController.StartLevel (activeIngredients, activeLevel);
	}

	private void SetVisibilityAllPanels(bool visibility) {
		SetVisibilityPanel (PANEL_BACKGROUND, visibility);
		SetVisibilityPanel (PANEL_LEVEL_SELECTION, visibility);
		SetVisibilityPanel (PANEL_SANDWICH, visibility);
		SetVisibilityPanel (PANEL_SELECTED_SANDWICH, visibility);
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
