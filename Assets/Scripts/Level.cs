using System;
using System.Collections.Generic;

public class Level
{
    public List<KeyValuePair<int, int>> enemyTypeAmount { get; set; }

    public int maxFailsForGameOver { get; set; }

    public int minKillsForWin { get; set; }

    public int spawnInterval { get; set; }

    public List<Ingredient> availableIngredients { get; set; }

    public List<KeyValuePair<Bread, int>> availableBreadsWithHits { get; set; }

}