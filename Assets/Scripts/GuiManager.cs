using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class GuiManager : MonoBehaviour
{
    public const string BUTTON_NEXT_SANWICH = "ButtonNextSandwich";
	public const string BUTTON_PREVIOUS_SANDWICH_LEVEL = "ButtonPreviousSandwich";

    public GameObject prefabButtonLevel;
    public GameObject panelBackground;
//    public GameObject panelLevelSelection;
    public GameObject panelSelectedSandwich;
    public GameObject panelSandwich;
	public GameObject panelWinLose;
	public GameObject panelWeaponSlot1;
	public GameObject panelWeaponSlot2;
	public GameObject panelIntroduction;
	public GameObject panelIntroductionSandwich;
	public GameObject panelIntroductionWeaponSwitch;
	public GameObject textGoToCounter;

//    private List<Level> levels;
    private GameFlowController gameFlowController;
    private Transform panelBread;
    private Transform panelIngredient1;
    // private Transform panelIngredient2;
    private Dropdown dropdownBread;
    private Dropdown dropdownIngredient1;
    // private Dropdown dropdownIngredient2;
	public GameObject panelEnemyVegan;
	public GameObject panelEnemyFattie;
    // TODO Add bread

    private Ingredient ingredient1;
    private Ingredient ingredient2;
    private KeyValuePair<Bread, int> bread;
    private Level activeLevel;

    private bool firstBread = true;
    List<Ingredient> ingredients1 = new List<Ingredient>();
    List<Ingredient> ingredients2 = new List<Ingredient>();
    private KeyValuePair<Bread, int> bread1;
    private KeyValuePair<Bread, int> bread2;
	private int currentLevelNumber;
	private int chosenWeapon = 1;

	public void Init(GameFlowController gameFlowController, Level level)
    {
		this.gameFlowController = gameFlowController;
        FindAndSetAllSubPanels();
        
		this.activeLevel = level;
		if (this.activeLevel == null) {
			Debug.LogError ("GuiManager activeLevel is null");
		}

		this.currentLevelNumber = gameFlowController.getSceneLevelNumber ();
		if (this.currentLevelNumber == -1) {
			Debug.LogError ("currentLevelNumber could not be loaded in GuiManager");
		}

		// Show Introduction just in the first level else show the enemytypes
		if (this.currentLevelNumber == 1) {
			ShowIntroduction ();
		}

		if (this.currentLevelNumber == 2) {
			ShowEnemyVegan ();
		}

		if (this.currentLevelNumber == 3) {
			ShowEnemyFattie ();
		}
    }

	private void ShowIntroduction() {
		SetVisibilityAllPanels (false);
		panelIntroduction.SetActive (true);
		SetVisibilityCursor(true);
		AddListenerToIntroductionNextButton ();
	}
		
	private void ShowIntroductionWeaponSwitch() {
		SetVisibilityAllPanels (false);
		panelIntroductionWeaponSwitch.SetActive (true);
		SetVisibilityCursor(true);
		AddListenerToIntroductionWeaponSwitchNextButton ();
	}

	private void ShowIntroductionSandwich() {
		SetVisibilityAllPanels (false);
		panelIntroductionSandwich.SetActive (true);
		SetVisibilityCursor(true);
		AddListenerToIntroductionSandwichNextButton ();
	}

	private void ShowEnemyVegan() {
		SetVisibilityAllPanels(false);
		panelEnemyVegan.SetActive(true);
		SetVisibilityCursor(true);
		AddListenerToEnemyVeganNextButton();
	}

	private void ShowEnemyFattie()
	{
		SetVisibilityAllPanels(false);
		panelEnemyFattie.SetActive(true);
		SetVisibilityCursor(true);
		AddListenerToEnemyFattieNextButton();
	}

	private void AddListenerToEnemyVeganNextButton() {
		Transform nextEnemyVeganTransform = panelEnemyVegan.transform.FindChild("ButtonNext");
		if (nextEnemyVeganTransform == null)
		{
			Debug.LogError("Next Button is null");
		}
		Button nextIntroductionButton = nextEnemyVeganTransform.GetComponent<Button>();
		nextIntroductionButton.onClick.AddListener(() => OnNextEnemyVeganButton());
	}

	private void AddListenerToEnemyFattieNextButton()
	{
		Transform nextEnemyFattieTransform = panelEnemyFattie.transform.FindChild("ButtonNext");
		if (nextEnemyFattieTransform == null)
		{
			Debug.LogError("Next Button is null");
		}
		Button nextEnemyFattieButton = nextEnemyFattieTransform.GetComponent<Button>();
		nextEnemyFattieButton.onClick.AddListener(() => OnNextEnemyFattieButton());
	}

	private void OnNextEnemyVeganButton()
	{
		// ShowIntroductionSandwich ();
		ShowIntroductionWeaponSwitch();
	}
//
	private void OnNextEnemyFattieButton()
	{
		ShowIntroductionSandwich ();
	}

	/**
   * Show the win or lose message and after the button "weiter" was clicked the level
   * selection will be shown.
   *
   **/
	public void ShowWinLoseMessageAndRestart(string text, bool won) {
		SetVisibilityAllPanels (false);
		panelWinLose.SetActive (true);
		SetWinLoseTextUI (text);
		SetVisibilityCursor (true);
		AddListenerToWinLoseNextButton (won);
	}

	private void SetWinLoseTextUI(string text) {
		// Get textfield of panel
		Transform textfield = panelWinLose.transform.FindChild("Text");
		Text textUi = textfield.GetComponent<Text> ();

		// Set text out of ingredients and bread choice
		textUi.text = text;
	}

	private void AddListenerToIntroductionNextButton() {
		Transform nextIntroductionTransform = panelIntroduction.transform.FindChild ("ButtonNext");
		if (nextIntroductionTransform == null) {
			Debug.LogError("Next Button is null");
		}
		Button nextIntroductionButton = nextIntroductionTransform.GetComponent<Button> ();
		nextIntroductionButton.onClick.AddListener (() => OnNextIntroductionButton());
	}

	private void AddListenerToIntroductionWeaponSwitchNextButton() {
		Transform nextIntroductionWeaponSwitchTransform = panelIntroductionWeaponSwitch.transform.FindChild ("ButtonNext");
		if (nextIntroductionWeaponSwitchTransform == null) {
			Debug.LogError("Next Button is null");
		}
		Button nextIntroductionWeaponSwitchButton = nextIntroductionWeaponSwitchTransform.GetComponent<Button> ();
		nextIntroductionWeaponSwitchButton.onClick.AddListener (() => OnNextIntroductionWeaponSwitchButton());
	}

	private void AddListenerToIntroductionSandwichNextButton() {
		Transform nextIntroductionTransform = panelIntroductionSandwich.transform.FindChild ("ButtonNext");
		if (nextIntroductionTransform == null) {
			Debug.LogError("Next Button is null");
		}
		Button nextIntroductionButton = nextIntroductionTransform.GetComponent<Button> ();
		nextIntroductionButton.onClick.AddListener (() => OnNextIntroductionSandwichButton());
	}



	private void AddListenerToWinLoseNextButton(bool won) {
		Transform nextTransform = panelWinLose.transform.FindChild("Button");
		if (nextTransform == null)
		{
			Debug.LogError("Next Button is null");
		}
		Button nextButton = nextTransform.GetComponent<Button> ();
		Transform mainTransform = panelWinLose.transform.FindChild("ButtonMainMenu");
		if (mainTransform == null)
		{
			Debug.LogError("Main Button is null");
		}
		Button mainMenuButton = mainTransform.GetComponent<Button> ();

		nextButton.onClick.RemoveAllListeners ();

		Transform nextButtonTrans = nextButton.transform.FindChild ("Text");
		Text nextButtonText = nextButtonTrans.GetComponent<Text> ();
		if (won) {
			nextButtonText.text = "Nächste Level";
			nextButton.onClick.AddListener (() => OnNextButtonWin ());
		} else {
			nextButtonText.text = "Level neustarten";
			nextButton.onClick.AddListener (() => OnNextButtonLose ());
		}

		mainMenuButton.onClick.RemoveAllListeners ();
		mainMenuButton.onClick.AddListener (() => OnMainMenuButtonClicked ());
	}

	private void OnNextButtonLose() {
		// Start current Level again
		OnNextIntroductionButton();
	}

	private void OnNextButtonWin() {
		if (currentLevelNumber == 1) {
			SceneManager.LoadScene (SceneKeys.SCENE_NAME_LEVEL_2);
		}
		if (currentLevelNumber == 2) {
			SceneManager.LoadScene (SceneKeys.SCENE_NAME_LEVEL_3);
		}
	}

	private void OnMainMenuButtonClicked() {
		SceneManager.LoadScene (SceneKeys.SCENE_NAME_MAIN_MENU);
	}

	private void OnNextIntroductionButton () {
		// First level means there is just one ingredient
		List<Ingredient> ingredients = new List<Ingredient>();
		ingredients.Add (activeLevel.availableIngredients [0]);
		// ingredients.Add (activeLevel.availableIngredients [0]);
		List<KeyValuePair<Bread, int>> bread = activeLevel.availableBreadsWithHits;
		StartLevel (ingredients, ingredients, bread [0], bread [0], activeLevel);
	}

	private void OnNextIntroductionWeaponSwitchButton () {
		// Second level means there are two ingredients
		List<Ingredient> ingredients = new List<Ingredient>();
		List<Ingredient> ingredients2 = new List<Ingredient>();
		// Choose ingredient chicken for the first sandwich
		// Choose tomato for the second
		ingredients.Add (activeLevel.availableIngredients [0]);
		ingredients2.Add (activeLevel.availableIngredients [1]);

		List<KeyValuePair<Bread, int>> bread = activeLevel.availableBreadsWithHits;
		StartLevel (ingredients, ingredients2, bread [0], bread [0], activeLevel);
	}

	private void OnNextIntroductionSandwichButton () {
		Debug.Log ("bla");
		firstBread = true;
		ShowSandwichCombinator (activeLevel);
	}

	public void SetVisibilityTextGoToCounter (bool visibility) {
		textGoToCounter.SetActive (visibility);
	}

	private void ShowSandwichCombinator(Level activeLevel)
    {
		SetVisibilityAllPanels (false);
        if (firstBread == true)
        {
			InitializeAllDropdownsWithValues(activeLevel);
			// No sandwich was chosen
			panelSelectedSandwich.SetActive(false);

			// Set headline
			SetSandwichCombinatorHeadlineText("1. Sandwich");
        } else {
            // one or more sandwiches are already available
            // so show PANEL_SELECTED_SANDWICH
			ShowChosenSandwichPanel(ingredients1, bread1);
			// Set headline
			SetSandwichCombinatorHeadlineText("2. Sandwich");
        }
		panelSandwich.SetActive (true);
		SetVisibilityNextSandwichButton(true, activeLevel);
		SetVisibilityPreviousSandwichLevelButton(true, activeLevel);
//		SetEffectsIngredientPanel1 ();
		SetDropdownBreadListener (panelBread.transform, dropdownBread);
		SetDropdownIngredientListener (panelIngredient1.transform, dropdownIngredient1);
		// SetDropdownIngredientListener (panelIngredient2.transform, dropdownIngredient2);
		List<KeyValuePair<Bread, int>> availableBreadWithHits = activeLevel.availableBreadsWithHits;
		KeyValuePair<Bread, int> chosenBread = availableBreadWithHits[dropdownBread.value];
		SetEffectsBreadPanel (panelBread.transform,
			chosenBread
		);
		List<Ingredient> availableIngredients = activeLevel.availableIngredients;
		Ingredient chosenIngredient = availableIngredients[dropdownIngredient1.value];
		SetEffectsIngredientPanel (panelIngredient1.transform, 
			chosenIngredient
		);
		// chosenIngredient = availableIngredients[dropdownIngredient2.value];
		/* SetEffectsIngredientPanel (panelIngredient2.transform, 
			chosenIngredient
		);*/
    }

	private void ShowWeaponPanels() {
		SetVisibilityAllPanels (false);
		panelWeaponSlot1.SetActive (true);
		panelWeaponSlot2.SetActive (true);
		SetChosenWeapon (1);
	}

	public void SetChosenWeapon(int weaponSlotNumber) {
		Color colorNotChosen = new Color();
		ColorUtility.TryParseHtmlString("#FFFFFF63", out colorNotChosen);
		Image img1 = panelWeaponSlot1.GetComponent<Image> ();
		Image img2 = panelWeaponSlot2.GetComponent<Image> ();
		switch (weaponSlotNumber) {
			case 1:
				img1.color = UnityEngine.Color.grey;
				img2.color = colorNotChosen;
				chosenWeapon = 1;
				break;
			case 2:
				img1.color = colorNotChosen;
				img2.color = UnityEngine.Color.grey;
				chosenWeapon = 2;
				break;
			default:
				img1.color = UnityEngine.Color.grey;
				img2.color = colorNotChosen;
				chosenWeapon = 1;
				break;
		}
	}

	public int GetChosenWeapon() {
		return chosenWeapon;
	}

	public void SetBread1Hits(int? breadHealth) {
		Transform textTrans1Hits = panelWeaponSlot1.transform.FindChild("TextHits");
		Text text1Hits = textTrans1Hits.GetComponent<Text> ();
		text1Hits.text = breadHealth.ToString ();
		if (breadHealth <= 0) {
			SetVisibilityWeaponPanel1 (false);
		}
        else SetVisibilityWeaponPanel1(true);
    }

	public void SetBread2Hits(int? breadHealth) {
		Transform textTrans2Hits = panelWeaponSlot2.transform.FindChild("TextHits");
		Text text2Hits = textTrans2Hits.GetComponent<Text> ();
		text2Hits.text = breadHealth.ToString ();
		if (breadHealth <= 0) {
			SetVisibilityWeaponPanel2 (false);
		}
        else SetVisibilityWeaponPanel2(true);
    }

	private void SetVisibilityWeaponPanel1(bool visibility) {
			panelWeaponSlot1.SetActive (visibility);

	}

	private void SetVisibilityWeaponPanel2(bool visibility) {
			panelWeaponSlot2.SetActive (visibility);
	}

	private void SetWeaponPanelsIngredientsText(List<Ingredient> ingredients1, List<Ingredient> ingredients2, KeyValuePair<Bread, int> bread1, KeyValuePair<Bread, int> bread2) {
		// Get textfield of panel
		Transform textTrans1 = panelWeaponSlot1.transform.FindChild("TextIngredients");
		Text text1 = textTrans1.GetComponent<Text> ();
		Transform textTrans2 = panelWeaponSlot2.transform.FindChild("TextIngredients");
		Text text2 = textTrans2.GetComponent<Text> ();
		Transform textTrans1Hits = panelWeaponSlot1.transform.FindChild("TextHits");
		Text text1Hits = textTrans1Hits.GetComponent<Text> ();
		Transform textTrans2Hits = panelWeaponSlot2.transform.FindChild("TextHits");
		Text text2Hits = textTrans2Hits.GetComponent<Text> ();

		// Set text out of ingredients and bread choice
		string textUi = "";
		textUi = bread1.Key.getName () + "\n";

		foreach (Ingredient ingredient in ingredients1) {
			textUi += ingredient.getName ()+"\n";
		}
		text1.text = textUi;
		// Set text out of ingredients and bread choice
		textUi = "";
		textUi = bread2.Key.getName () + "\n";

		foreach (Ingredient ingredient in ingredients2) {
			textUi += ingredient.getName ()+"\n";
		}
		text2.text = textUi;

		// Set the hit value of the bread
		text1Hits.text = bread1.Value.ToString ();
		text2Hits.text = bread2.Value.ToString ();
	}

	private void SetSandwichCombinatorHeadlineText(string text) {
		Transform textTransform = panelSandwich.transform.FindChild ("TextHeadline");
		Text headlineText = textTransform.GetComponent<Text> ();
		headlineText.text = text;
	}

	private void ShowChosenSandwichPanel(List<Ingredient> ingredients1, KeyValuePair<Bread, int> bread1) {
		SetVisibilityAllPanels (false);
		panelSelectedSandwich.SetActive (true);

		// Get textfield of panel
		Transform textfield = panelSelectedSandwich.transform.FindChild("TextConstilation");
		Text text = textfield.GetComponent<Text> ();

		// Set text out of ingredients and bread choice
		string textUi = "";
		textUi = bread1.Key.getName () + "\n";

		foreach (Ingredient ingredient in ingredients1) {
			textUi += ingredient.getName ()+"\n";
		}
		text.text = textUi;
	}

//    private void SetDropdownBreadListener(Dropdown dropdownBread, int chosenLevel)
//    {
//        if (dropdownBread == null)
//        {
//            Debug.LogError("dropdown bread is null");
//        }
//        // Set an empty start up value
//        //SetBlankDefaultDropdownValue(dropdownBread);
//        // Set onChange listener
//        dropdownBread.onValueChanged.AddListener(delegate
//        {
//            OnBreadValueChanged(dropdownBread.value, chosenLevel);
//        });
//        OnBreadValueChanged(0, chosenLevel);
//    }

//    private void OnBreadValueChanged(int chosenItem, int chosenLevel)
//    {
//		chosenLevel = levels.IndexOf (activeLevel);
//		Level level = this.levels[chosenLevel];
//        List<KeyValuePair<Bread, int>> availableBreadsWithHits = level.availableBreadsWithHits;
//        bread = availableBreadsWithHits[chosenItem];
//    }

	private void SetDropdownBreadListener(Transform panelTransform, Dropdown dropdown) {
		// Set onChange listener
		dropdown.onValueChanged.AddListener(delegate
			{
				OnBreadValueChanged(dropdown.value, panelTransform, dropdown);
			});
	}

	private void SetDropdownIngredientListener(Transform panelTransform, Dropdown dropdown) {
        // Set onChange listener
		dropdown.onValueChanged.AddListener(delegate
        {
				OnIngredientValueChanged(dropdown.value, panelTransform, dropdown);
        });
    }

	private void OnBreadValueChanged(int chosenItem, Transform panelTransform, Dropdown dropdown)
	{
		List<KeyValuePair<Bread, int>> availableBreadWithHits = activeLevel.availableBreadsWithHits;
		KeyValuePair<Bread, int> chosenBread = availableBreadWithHits[chosenItem];
		SetEffectsBreadPanel (panelTransform,
			chosenBread
		);
	}

	private void OnIngredientValueChanged(int chosenItem, Transform panelTransform, Dropdown dropdown)
    {
		List<Ingredient> availableIngredients = activeLevel.availableIngredients;
		Ingredient chosenIngredient = availableIngredients[chosenItem];
		SetEffectsIngredientPanel (panelTransform,
			chosenIngredient
		);
    }

	private void SetEffectsBreadPanel (
		Transform panelBread,
		KeyValuePair<Bread, int> chosenBread
	) {
		Transform panelEffectsTrans = panelBread.transform.FindChild ("PanelEffects");
		Transform textDamageTrans = panelEffectsTrans.FindChild ("TextDamage");
		Text textDamage = textDamageTrans.GetComponent<Text> ();
		Transform textHitsTrans = panelEffectsTrans.FindChild ("TextHits");
		Text textHits = textHitsTrans.GetComponent<Text> ();

		textDamage.text = chosenBread.Key.getBaseDamage ().ToString () + " Schaden";
		textHits.text = chosenBread.Value.ToString () + " Haltbarkeit";
	}

	private void SetEffectsIngredientPanel(
		Transform panelIngredient, 
		Ingredient chosenIngredient
	) {
		Transform panelEffectsTrans = panelIngredient.transform.FindChild ("PanelEffects");
		Transform textVegiPosTrans = panelEffectsTrans.FindChild ("TextVegiPos");
		Text textVegiPos = textVegiPosTrans.GetComponent<Text> ();
		Transform textVegiNegTrans = panelEffectsTrans.FindChild ("TextVegiNeg");
		Text textVegiNeg = textVegiNegTrans.GetComponent<Text> ();
		Transform textMeatPosTrans = panelEffectsTrans.FindChild ("TextMeatPos");
		Text textMeatPos = textMeatPosTrans.GetComponent<Text> ();
		Transform textMeatNegTrans = panelEffectsTrans.FindChild ("TextMeatNeg");
		Text textMeatNeg = textMeatNegTrans.GetComponent<Text> ();

		int speedBonusVegi = chosenIngredient.getSpeedBonus (Enemy.VEGAN);
		int speedBonusMeat = chosenIngredient.getSpeedBonus (Enemy.FAT);
		int damageBonusVegi = chosenIngredient.getDamageBonus(Enemy.VEGAN);
		int damageBonusMeat = chosenIngredient.getDamageBonus(Enemy.FAT);

		string speedBonusVegiTxt = "";
		string speedBonusMeatTxt = "";
		string damageBonusVegiTxt = "";
		string damageBonusMeatTxt = "";

		if (speedBonusMeat > 0)
			speedBonusMeatTxt = "+";
		if (speedBonusVegi > 0)
			speedBonusVegiTxt = "+";
		if (damageBonusMeat > 0)
			damageBonusMeatTxt = "+";
		if (damageBonusVegi > 0)
			damageBonusVegiTxt = "+";
		speedBonusVegiTxt += speedBonusVegi.ToString () + " Geschwindigkeit";
		speedBonusMeatTxt += speedBonusMeat.ToString () + " Geschwindigkeit";
		damageBonusVegiTxt += damageBonusVegi.ToString () + " Schaden";
		damageBonusMeatTxt += damageBonusMeat.ToString () + " Schaden";

		if(chosenIngredient.getType().Equals(IngredientType.MEAT)) {
			textVegiPos.text = damageBonusVegiTxt;
			textVegiNeg.text = speedBonusVegiTxt;
			textMeatPos.text = speedBonusMeatTxt;
			textMeatNeg.text = damageBonusMeatTxt;
		} else if(chosenIngredient.getType().Equals(IngredientType.VEGETABLE)) {
			textVegiPos.text = speedBonusVegiTxt;
			textVegiNeg.text = damageBonusVegiTxt;
			textMeatPos.text = damageBonusMeatTxt;
			textMeatNeg.text = speedBonusMeatTxt;
		}
	}

    private void SetBlankDefaultDropdownValue(Dropdown dropdown)
    {
        //It is a hacky solution but I didn't find an other one
        // Add a blank dropdown option you will then remove at the end of the options list
        dropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData() { text = "" });
        // Select it
        dropdown.GetComponent<Dropdown>().value = dropdown.GetComponent<Dropdown>().options.Count - 1;
        // Remove it
        dropdown.GetComponent<Dropdown>().options.RemoveAt(dropdown.GetComponent<Dropdown>().options.Count - 1);
    }

    private void InitializeAllDropdownsWithValues(Level activeLevel)
    {
		// Get all dropdowns
        FindAndSetAllDropdowns();
		Level level = activeLevel;

        List<string> breads = new List<string>();
        List<KeyValuePair<Bread, int>> availableBreadsWithHits = level.availableBreadsWithHits;
        foreach (KeyValuePair<Bread, int> entry in availableBreadsWithHits)
        {
            breads.Add(entry.Key.getName());
        }

        // Get and set all available ingredients
        List<string> optionsTmp = new List<string>();
        List<Ingredient> ingredients = level.availableIngredients;
        foreach (Ingredient ingredient in ingredients)
        {
			optionsTmp.Add(ingredient.getName());
        }
        dropdownBread.ClearOptions();
        // Just an example case
        dropdownBread.AddOptions(breads);

        // Add options
        dropdownIngredient1.ClearOptions();
        dropdownIngredient1.AddOptions(optionsTmp);
        // dropdownIngredient2.ClearOptions();
        // dropdownIngredient2.AddOptions(optionsTmp);
    }

	private void SetVisibilityPreviousSandwichLevelButton(bool visible, Level activeLevel)
    {
        Transform previousButton = panelSandwich.transform.FindChild("ButtonPreviousSandwich");
        if (previousButton == null)
        {
            Debug.LogError("Previous Button is null");
        }
		Button previous = previousButton.GetComponent<Button> ();
		previous.onClick.RemoveAllListeners();
		previous.onClick.AddListener(() => OnPreviousSandwichButtonClicked(this.activeLevel));
        previousButton.gameObject.SetActive(visible);
    }

	private void SetVisibilityNextSandwichButton(bool visible, Level activeLevel)
    {
		Transform nextTransform = panelSandwich.transform.FindChild("ButtonNextSandwich");
        if (nextTransform == null)
        {
            Debug.LogError("Previous Button is null");
        }
		Button nextButton = nextTransform.GetComponent<Button> ();
		nextButton.onClick.RemoveAllListeners ();
		nextButton.onClick.AddListener(() => OnNextSandwichButtonClicked(activeLevel));
        nextTransform.gameObject.SetActive(visible);
    }

    private void SetVisibilityCursor(bool visible)
    {
        GameObject player = GameObject.Find("FirstPersonCharacter");
        player.GetComponent<Crosshair>().enabled = !visible;
        player.GetComponent<FirstPersonController>().enabled = !visible;
        player.GetComponentInChildren<WeaponController>().enabled = !visible;
        Cursor.visible = visible;
        Cursor.lockState = CursorLockMode.None;
    }

//    private void AddButtonToLevelSelection(int unlockedLevelsCount)
//    {
//		GameObject panel = panelLevelSelection;
//        RectTransform panelRect = panel.GetComponentInChildren<RectTransform>();
//        foreach (Transform child in panel.transform)
//        {
//            GameObject.Destroy(child.gameObject);
//        }
//
//        foreach (Level level in levels)
//        {
//            int index = levels.IndexOf(level);
//            if (index < unlockedLevelsCount)
//            {
//                GameObject goButton = Instantiate(prefabButtonLevel) as GameObject;
//                goButton.transform.SetParent(panelRect, false);
//                goButton.GetComponentInChildren<Text>().text = "Level " + (index + 1);
//                goButton.GetComponentInChildren<Text>().fontSize = 16;
//				goButton.GetComponent<Button>().onClick.AddListener(() => OnLevelButtonClicked(index, unlockedLevelsCount));
//            }
//            else
//            {
//                break;
//            }
//        }
//    }

//	private void OnLevelButtonClicked(int chosenLevel, int unlockedLevelsCount)
//    {
//        SetActiveLevel(chosenLevel);
//		firstBread = true;
//		ingredients1.Clear ();
//		ingredients2.Clear ();
//		ShowSandwichCombinator(unlockedLevelsCount, levels.IndexOf(activeLevel));
//    }

//    private void SetActiveLevel(int chosenLevel)
//    {
//        this.activeLevel = levels[chosenLevel];
//    }

	private void OnNextSandwichButtonClicked(Level activeLevel)
    {
		// Get data from all Dropdowns of sandwichchooser
		GetAndSetSandwichDataFromAllDropdowns ();
		// if (ingredient1 != null && ingredient2 != null && activeLevel != null)
		if (ingredient1 != null && activeLevel != null)
        {
            if (firstBread == true)
            {
                ingredients1.Add(ingredient1);
                // ingredients1.Add(ingredient2);
                bread1 = bread;
                firstBread = false;
				ShowSandwichCombinator(activeLevel);
            }
            else
            {
                ingredients2.Add(ingredient1);
                // ingredients2.Add(ingredient2);
                bread2 = bread;
                StartLevel(ingredients1, ingredients2, bread1, bread2, activeLevel);
            }
        }
        else
        {
            Debug.Log("wait");
        }
    }

	private void GetAndSetSandwichDataFromAllDropdowns() {
		// When "Next" Button got clicked put the choice in the lists
		int breadId = dropdownBread.value;
		int ingredient1Id = dropdownIngredient1.value;
		// int ingredient2Id = dropdownIngredient2.value;

		Level level = this.activeLevel;
		List<Ingredient> availableIngredients = level.availableIngredients;
		List<KeyValuePair<Bread, int>> availableBreadsWithHits = level.availableBreadsWithHits;
		bread = availableBreadsWithHits[breadId];
		ingredient1 = availableIngredients[ingredient1Id];
		// ingredient2 = availableIngredients[ingredient2Id];
	}

	private void OnPreviousSandwichButtonClicked(Level activeLevel) {
		if (firstBread == true) {
			// TODO Show Enemytypes or hide previous Button
		} else {
			ingredients1.Clear ();
			ingredients2.Clear ();
			firstBread = true;
			ShowSandwichCombinator (activeLevel);
		}
	}

    private void StartLevel(List<Ingredient> ingredients1, List<Ingredient> ingredients2, KeyValuePair<Bread, int> bread1, KeyValuePair<Bread, int> bread2, Level activeLevel)
    {
        SetVisibilityCursor(false);
        SetVisibilityAllPanels(false);
		SetWeaponPanelsIngredientsText (ingredients1, ingredients2, bread1, bread2);
		ShowWeaponPanels ();
        gameFlowController.StartLevel(ingredients1, ingredients2, bread1, bread2, activeLevel);
    }

    private void SetVisibilityAllPanels(bool visibility)
    {
		panelBackground.SetActive (visibility);
		panelSandwich.SetActive (visibility);
		panelSelectedSandwich.SetActive (visibility);
		panelWinLose.SetActive (visibility);
		panelWeaponSlot1.SetActive (visibility);
		panelWeaponSlot2.SetActive (visibility);
		textGoToCounter.SetActive (visibility);
		panelEnemyVegan.SetActive(visibility);
		panelEnemyFattie.SetActive(visibility);
		panelIntroduction.SetActive (visibility);
		panelIntroductionSandwich.SetActive (visibility);
		panelIntroductionWeaponSwitch.SetActive (visibility);
    }

    private void FindAndSetAllSubPanels()
    {
        // Get the sub panels
        Transform panelSandwichTransform = panelSandwich.transform;
        panelBread = panelSandwichTransform.FindChild("PanelBread");
        Transform panelIngredient1Transform = panelSandwich.transform;
        panelIngredient1 = panelIngredient1Transform.FindChild("PanelIngredient1");
        // Transform panelIngredient2Transform = panelSandwich.transform;
        // panelIngredient2 = panelIngredient2Transform.FindChild("PanelIngredient2");
        if (panelBread == null ||
			panelIngredient1 == null) // ||
            // panelIngredient2 == null)
        {
            Debug.LogError("A sub panel is null");
        }
    }

    private void FindAndSetAllDropdowns()
    {
        string dropdownText = "Dropdown";
        dropdownBread = panelBread.FindChild(dropdownText).GetComponent<Dropdown>();
        dropdownIngredient1 = panelIngredient1.FindChild(dropdownText).GetComponent<Dropdown>();
        // dropdownIngredient2 = panelIngredient2.FindChild(dropdownText).GetComponent<Dropdown>();
        if (dropdownBread == null ||
			dropdownIngredient1 == null) // ||
           // dropdownIngredient2 == null)
        {
            Debug.LogError("A dropdown is null");
        }
    }
}
