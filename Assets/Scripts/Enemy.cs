using System.Collections.Generic;

public class Enemy
{
    public const int NORMAL = 2000;
    public const int VEGAN = 2001;

    public static Dictionary<int, int> HealthList = new Dictionary<int, int> { { NORMAL, 50 }, { VEGAN, 30 } };

    public static Dictionary<int, Dictionary<int, int>> IngredientBonus = new Dictionary<int, Dictionary<int, int>>
    {
        {
            NORMAL,
            new Dictionary<int, int>
            {
                {Ingredient.WHITE_BREAD, 5},
                {Ingredient.BUTTER, 0}
            }
        },
        {
            VEGAN,
            new Dictionary<int, int>
            {
                {Ingredient.WHITE_BREAD, 10},
                {Ingredient.BUTTER, 10}
            }
        }
    };
}