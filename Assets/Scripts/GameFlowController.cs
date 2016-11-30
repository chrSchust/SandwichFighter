using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;


public class GameFlowController : MonoBehaviour
{

    private int unlockedLevelsCount = 0;
    private Level activeLevel;
    private List<Level> levels;
    private int fails = 0;
    private int kills = 0;
    private List<Ingredient> activeIngredientList = new List<Ingredient>();
	private GuiManager guiManager = null;

    public RectTransform parentPanel;
    public GameObject prefabButton;


    // Use this for initialization
    void Start()
    {
		guiManager = GameObject.FindGameObjectWithTag ("GuiManager").GetComponent<GuiManager> ();
		if (guiManager == null) {
			Debug.LogError ("GuiManager is null");
		}
//        GameObject panel = GameObject.Find("Panel");
//        panel.GetComponent<CanvasGroup>().alpha = 0f;
//        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        initLevels();
		guiManager.Init (levels);
//        showLevelSelecetionUI();
    }

    private void initLevels()
    {
        Salami salami = new Salami();
        Chicken chicken = new Chicken();
        Salad salad = new Salad();
        Tomato tomato = new Tomato();
        levels = new List<Level>()
                {
                  new Level {enemyTypeAmount = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Enemy.NORMAL, 3)},
                      maxFailsForGameOver = 2,
                      minKillsForWin = 1,
                      availableIngredients = new List<Ingredient>() {chicken, tomato},
                      spawnInterval = 5
                },
                    new Level {enemyTypeAmount = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Enemy.NORMAL, 1), new KeyValuePair<int, int>(Enemy.VEGAN, 2), new KeyValuePair<int, int>(Enemy.NORMAL, 1)},
                      maxFailsForGameOver = 2,
                      minKillsForWin = 2,
                      availableIngredients = new List<Ingredient>() {chicken, tomato, salad, salami},
                      spawnInterval = 2
                }};
    }

//    private void showLevelSelecetionUI()
//    {
//        GameObject panel = GameObject.Find("Panel");
//        foreach (Transform child in panel.transform)
//        {
//            GameObject.Destroy(child.gameObject);
//        }
//        panel.GetComponent<CanvasGroup>().alpha = 1f;
//        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;

//        GameObject player = GameObject.Find("FirstPersonCharacter");
//        player.GetComponent<FirstPersonController>().enabled = false;
//        Cursor.visible = true;
//        Cursor.lockState = CursorLockMode.None;

//        foreach (Level level in levels)
//        {
//            int index = levels.IndexOf(level);
//            GameObject goButton = Instantiate(prefabButton) as GameObject;
//            goButton.transform.SetParent(parentPanel, false);
//            goButton.GetComponentInChildren<Text>().text = "Level " + (index + 1);
//            goButton.GetComponentInChildren<Text>().fontSize = 50;
//            goButton.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(index));
//        }

//    }
	public List<Level> GetLevels() {
		return levels;
	}

	public void statusFirstPersonController(bool status) {
		GameObject player = GameObject.Find("FirstPersonCharacter");
		player.GetComponent<FirstPersonController>().enabled = status;
	}

    public void ButtonClicked(int buttonNo)
    {
        setActiveLevel(buttonNo);
//        showIngredientsSelectionUI();
    }

	public int GetUnlockedLevelsCount() {
		return this.unlockedLevelsCount;
	}

    //call from level selection UI 
    private void setActiveLevel(int levelNumber)
    {
        activeLevel = levels[levelNumber];
    }

//    private void showIngredientsSelectionUI()
//    {
//        GameObject.Find("LevelEnd").GetComponent<Text>().text = "";
//
//        GameObject panel = GameObject.Find("Panel");
//        foreach (Transform child in panel.transform)
//        {
//            GameObject.Destroy(child.gameObject);
//        }
//        panel.GetComponent<CanvasGroup>().alpha = 1f;
//        panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
//
//        GameObject player = GameObject.Find("FirstPersonCharacter");
//        player.GetComponent<FirstPersonController>().enabled = false;
//
//        foreach (Ingredient ingredient in activeLevel.availableIngredients)
//        {
//            GameObject goButton = Instantiate(prefabButton) as GameObject;
//            goButton.transform.SetParent(parentPanel, false);
//            goButton.GetComponentInChildren<Text>().text = ingredient.getName();
//            goButton.GetComponentInChildren<Text>().fontSize = 50;
//            Ingredient tempIngredient = ingredient;
//            goButton.GetComponent<Button>().onClick.AddListener(() => IngredientButtonClicked(tempIngredient, goButton));
//        }
//        GameObject startButton = Instantiate(prefabButton) as GameObject;
//        startButton.transform.SetParent(parentPanel, false);
//        startButton.GetComponentInChildren<Text>().text = "START";
//        startButton.GetComponentInChildren<Text>().fontSize = 50;
//        startButton.GetComponent<Button>().onClick.AddListener(() => StartButtonClicked());
//    }

//    private void StartButtonClicked()
//    {
//        GameObject panel = GameObject.Find("Panel");
//        panel.GetComponent<CanvasGroup>().alpha = 0f;
//        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
//
//        GameObject player = GameObject.Find("FirstPersonCharacter");
//        player.GetComponent<FirstPersonController>().enabled = true;
//        startLevel();
//    }

//    private void IngredientButtonClicked(Ingredient ingredient, GameObject button)
//    {
//        button.GetComponent<Image>().color = Color.green;
//        activeIngredientList.Add(ingredient);
//    }

    //call from ingredients selection UI 
    private void startLevel()
    {
        GameObject weapon = GameObject.Find("WeaponHitPoint");
		weapon.GetComponent<MeleeAttack>().ingredients = activeIngredientList;

        GameObject spawner = GameObject.Find("Spawner");
        StartCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
    }

    internal void checkGameOver()
    {
        fails++;
        if (fails == activeLevel.maxFailsForGameOver)
        {
            GameObject.Find("LevelEnd").GetComponent<Text>().text = "Game Over";
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                GameObject.Destroy(enemy);
            }
//            showGameOverUI();
            fails = 0;
            kills = 0;
            activeIngredientList = new List<Ingredient>();
            GameObject spawner = GameObject.Find("Spawner");
            StopCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
        }
    }

//    private void showGameOverUI()
//    {
//        showLevelSelecetionUI();
//    }

    internal void checkWon()
    {
        kills++;
        if (kills == activeLevel.minKillsForWin)
        {
            GameObject.Find("LevelEnd").GetComponent<Text>().text = "Level Cleared";
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                GameObject.Destroy(enemy);
            }
            unlockedLevelsCount++;
//            showLevelClearedUI();
            kills = 0;
            fails = 0;
            activeIngredientList = new List<Ingredient>();
            GameObject spawner = GameObject.Find("Spawner");
            StopCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
        }
    }

//    private void showLevelClearedUI()
//    {
//        showLevelSelecetionUI();
//    }

    // Update is called once per frame
    void Update()
    {

    }
}
