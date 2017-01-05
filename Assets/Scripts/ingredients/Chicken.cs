
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
                // return -5;
				return 0;
            case Enemy.VEGAN:
                // return +5;
				return +10;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return -2;
				return -1;
            case Enemy.VEGAN:
                // return +2;
				return +1;
            default: return 0;
        }
    }
}
