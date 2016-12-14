
using System;

public class Chicken : Ingredient
{
    public string getName()
    {
        return "Hähnchen";
    }

	public string getType() 
	{ 
		return IngredientType.MEAT; 
	}

    public int getDamageBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                return -5;
            case Enemy.VEGAN:
                return +5;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                return -2;
            case Enemy.VEGAN:
                return +2;
            default: return 0;
        }
    }
}
