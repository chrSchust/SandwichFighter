using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameFlowController : MonoBehaviour
{

    private int unlockedLevelsCount = 0;
    private Level activeLevel;
    private List<Level> levels;
    private int fails = 0;
    private int kills = 0;

    // Use this for initialization
    void Start()
    {
        initLevels();
        showLevelSelecetionUI();

        //remove these when ui implemented
        setActiveLevel(1);
        startLevel();
    }

    private void initLevels()
    {
        levels = new List<Level>()
                {
                  new Level {enemyTypeAmount = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Enemy.NORMAL, 3)},
                      maxFailsForGameOver = 2,
                      minKillsForWin = 1,
                      availableIngredients = new List<int>() {Ingredient.WHITE_BREAD},
                      spawnInterval = 5
                },
                    new Level {enemyTypeAmount = new List<KeyValuePair<int, int>>() { new KeyValuePair<int, int>(Enemy.NORMAL, 1), new KeyValuePair<int, int>(Enemy.VEGAN, 2), new KeyValuePair<int, int>(Enemy.NORMAL, 1)},
                      maxFailsForGameOver = 2,
                      minKillsForWin = 2,
                      availableIngredients = new List<int>() {Ingredient.WHITE_BREAD, Ingredient.BUTTER},
                      spawnInterval = 2
                }};
    }

    private void showLevelSelecetionUI()
    {
        //use unlockedLevelsCount here
    }

    //call from level selection UI 
    private void setActiveLevel(int levelNumber)
    {
        activeLevel = levels[levelNumber];
        showIngredientsSelectionUI();
    }

    private void showIngredientsSelectionUI()
    {
        //use activeLevel.availableIngredients here
    }

    //call from ingredients selection UI 
    private void startLevel()
    {
        GameObject weapon = GameObject.Find("Weapon");
        weapon.GetComponent<WeaponController>().ingredients = new List<int> { Ingredient.WHITE_BREAD, Ingredient.BUTTER };

        GameObject spawner = GameObject.Find("Spawner");
        StartCoroutine(spawner.GetComponent<SpawnController>().spawn(activeLevel));
    }

    internal void checkGameOver()
    {
        fails++;
        if (fails == activeLevel.maxFailsForGameOver)
        {
            showGameOverUI();
        }
    }

    private void showGameOverUI()
    {
        Debug.Log("Game Over");
    }

    internal void checkWon()
    {
        kills++;
        if (kills == activeLevel.minKillsForWin)
        {
            Debug.Log("Level cleared");
            unlockedLevelsCount++;
            showLevelSelecetionUI();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
