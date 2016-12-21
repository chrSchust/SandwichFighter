using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using System.Reflection;

public class GameFlowController : MonoBehaviour
{

    private int unlockedLevelsCount = 3;
    private Level activeLevel;
    private List<Level> levels;
    private int fails = 0;
    private int kills = 0;
    private List<Ingredient> activeIngredientList = new List<Ingredient>();
    private GuiManager guiManager = null;
    private Coroutine spawnMethod;

    private GameObject player;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    public RectTransform parentPanel;
    public GameObject prefabButton;
    public GameObject GuiManagerPrefab;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("FirstPersonCharacter");
        playerStartPosition = player.transform.position;
        playerStartRotation = player.transform.rotation;

        guiManager = GameObject.FindGameObjectWithTag("GuiManager").GetComponent<GuiManager>();
        if (guiManager == null)
        {
            Debug.LogError("GuiManager is null");
        }
        //        GameObject panel = GameObject.Find("Panel");
        //        panel.GetComponent<CanvasGroup>().alpha = 0f;
        //        panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        initLevels();
        // Get the current scene to determine which level was chosen

        guiManager.Init(this, levels, getSceneLevelNumber());
        //        showLevelSelecetionUI();
    }

    private int getSceneLevelNumber()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;

        switch (sceneName)
        {
            case SceneKeys.SCENE_NAME_LEVEL_1:
                return 1;
            case SceneKeys.SCENE_NAME_LEVEL_2:
                return 2;
            case SceneKeys.SCENE_NAME_LEVEL_3:
                return 3;
            default:
                return -1;
        }
    }

    private void initLevels()
    {
        Salami salami = new Salami();
        Chicken chicken = new Chicken();
        Salad salad = new Salad();
        Tomato tomato = new Tomato();

        White white = new White();
        WholeGrain wholeGrain = new WholeGrain();

        levels = new List<Level>(){
                 new Level {
                    enemyTypeAmount = new List<KeyValuePair<int, int>>() {
                        new KeyValuePair<int, int>(Enemy.NORMAL, 10)
                    },
                    maxFailsForGameOver = 1,
                    minKillsForWin = 10,
                    availableIngredients = new List<Ingredient>() {chicken},
                    spawnInterval = 3,
                    availableBreadsWithHits = new List<KeyValuePair<Bread, int>>() {
                        new KeyValuePair<Bread, int>(white, 30)
                    }
                },
                new Level {
                    enemyTypeAmount = new List<KeyValuePair<int, int>>() {
                        new KeyValuePair<int, int>(Enemy.VEGAN, 5),
                        new KeyValuePair<int, int>(Enemy.NORMAL, 5),
                        new KeyValuePair<int, int>(Enemy.VEGAN, 5),
                        new KeyValuePair<int, int>(Enemy.NORMAL, 5)
                    },
                    maxFailsForGameOver = 1,
                    minKillsForWin = 20,
                    availableIngredients = new List<Ingredient>() {chicken, tomato},
                    spawnInterval = 3,
                    availableBreadsWithHits = new List<KeyValuePair<Bread, int>>() {
                        new KeyValuePair<Bread, int>(white, 30),
                        new KeyValuePair<Bread, int>(wholeGrain, 20)
                    }
                },
                new Level {
                    enemyTypeAmount = new List<KeyValuePair<int, int>>() {
                        new KeyValuePair<int, int>(Enemy.NORMAL, 5),
                        new KeyValuePair<int, int>(Enemy.VEGAN, 5),
                        new KeyValuePair<int, int>(Enemy.FAT, 1),
                        new KeyValuePair<int, int>(Enemy.NORMAL, 5),
                        new KeyValuePair<int, int>(Enemy.FAT, 1),
                        new KeyValuePair<int, int>(Enemy.VEGAN, 5)
                    },
                    maxFailsForGameOver = 1,
                    minKillsForWin = 22,
                    availableIngredients = new List<Ingredient>() {chicken, tomato, salad, salami},
                    spawnInterval = 1,
                    availableBreadsWithHits = new List<KeyValuePair<Bread, int>>() {
                        new KeyValuePair<Bread, int>(white, 30),
                        new KeyValuePair<Bread, int>(wholeGrain, 20)
                    }
                }
        };
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
    public List<Level> GetLevels()
    {
        return levels;
    }

    public void statusFirstPersonController(bool status)
    {
        GameObject player = GameObject.Find("FirstPersonCharacter");
        player.GetComponent<FirstPersonController>().enabled = status;
    }

    public void ButtonClicked(int buttonNo)
    {
        setActiveLevel(buttonNo);
        //        showIngredientsSelectionUI();
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
    //    private void startLevel()
    //    {
    //        GameObject weapon = GameObject.Find("WeaponHitPoint");
    //		weapon.GetComponent<MeleeAttack>().ingredients = activeIngredientList;
    //
    //        GameObject spawner = GameObject.Find("Spawner");
    //        StartCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
    //    }

    public void StartLevel(List<Ingredient> ingredientList1, List<Ingredient> ingredientList2, KeyValuePair<Bread, int> bread1, KeyValuePair<Bread, int> bread2, Level activeLevel)
    {
        this.activeLevel = activeLevel;
        GameObject weapon = GameObject.Find("WeaponHitPoint");
        weapon.GetComponent<MeleeAttack>().ingredients1 = ingredientList1;
        weapon.GetComponent<MeleeAttack>().ingredients2 = ingredientList2;
        weapon.GetComponent<MeleeAttack>().activeIngredients = ingredientList1;
        weapon.GetComponent<MeleeAttack>().bread1 = bread1.Key;
        weapon.GetComponent<MeleeAttack>().breadHealth1 = bread1.Value;
        //weapon.GetComponent<MeleeAttack>().bread2 = bread2.Key;

        Type t = bread2.Key.GetType();
        Assembly a = Assembly.GetAssembly(t);
        Bread newObject = (Bread)a.CreateInstance(t.FullName);
        weapon.GetComponent<MeleeAttack>().bread2 = newObject;

        weapon.GetComponent<MeleeAttack>().breadHealth2 = bread2.Value;
        weapon.GetComponent<MeleeAttack>().activebread = bread1.Key;

        guiManager.SetBread1Hits(bread1.Value);
        guiManager.SetBread2Hits(bread2.Value);

        GameObject spawner = GameObject.Find("Spawner");
        spawnMethod = StartCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
    }

    internal void checkGameOver()
    {
        fails++;
        if (fails == activeLevel.maxFailsForGameOver)
        {
            // GameObject.Find("LevelEnd").GetComponent<Text>().text = "Game Over";
            // TODO Have to be linked with GUIManager for now just use Debug message
            Debug.Log("Game over");
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                GameObject.Destroy(enemy);
            }
            //            showGameOverUI();

            //            GameObject guiManagerGo = GameObject.FindGameObjectWithTag("GuiManager");
            //            GameObject.Destroy(guiManagerGo);
            //            guiManager = Instantiate(GuiManagerPrefab).GetComponent<GuiManager>();
            //            guiManager.Init(this, levels, unlockedLevelsCount);
            guiManager.SetBread1Hits(0);
            guiManager.SetBread2Hits(0);
            guiManager.ShowWinLoseMessageAndRestart("Game Over!", levels, unlockedLevelsCount);

            player.transform.position = playerStartPosition;
            player.transform.rotation = playerStartRotation;

            //GameObject.Find("Weapon").GetComponentInChildren<Renderer>().enabled = true;

            fails = 0;
            kills = 0;
            activeIngredientList = new List<Ingredient>();
            GameObject spawner = GameObject.Find("Spawner");
            StopCoroutine(spawnMethod);
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
            // GameObject.Find("LevelEnd").GetComponent<Text>().text = "Level Cleared";
            // TODO Have to be linked with GUIManager for now just use Debug message
            Debug.Log("Level cleared");
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                GameObject.Destroy(enemy);
            }
            Debug.Log(PlayerPrefs.GetInt(SceneKeys.PLAYER_PREF_KEY_UNLOCKED_LEVELS, 0));
            if (PlayerPrefs.GetInt(SceneKeys.PLAYER_PREF_KEY_UNLOCKED_LEVELS, 0) == levels.IndexOf(activeLevel))
            {
                PlayerPrefs.SetInt(SceneKeys.PLAYER_PREF_KEY_UNLOCKED_LEVELS, levels.IndexOf(activeLevel)+1);
                Debug.Log(PlayerPrefs.GetInt(SceneKeys.PLAYER_PREF_KEY_UNLOCKED_LEVELS, 0));
            }
            //            showLevelClearedUI();

            //            GameObject guiManagerGo = GameObject.FindGameObjectWithTag("GuiManager");
            //            GameObject.Destroy(guiManagerGo);
            //            guiManager = Instantiate(GuiManagerPrefab).GetComponent<GuiManager>();
            //            guiManager.Init(this, levels, unlockedLevelsCount);
            guiManager.SetBread1Hits(0);
            guiManager.SetBread2Hits(0);
            guiManager.ShowWinLoseMessageAndRestart("Win!", levels, unlockedLevelsCount);

            player.transform.position = playerStartPosition;
            player.transform.rotation = playerStartRotation;

            //GameObject.Find("Weapon").GetComponentInChildren<Renderer>().enabled = true;

            kills = 0;
            fails = 0;
            activeIngredientList = new List<Ingredient>();
            GameObject spawner = GameObject.Find("Spawner");
            StopCoroutine(spawnMethod);
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
