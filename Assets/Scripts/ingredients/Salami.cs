
using System;

public class Salami : Ingredient
{
	public string getName()
    {
        return "Salami";
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
                // return -10;
				return 0;
            case Enemy.VEGAN:
                // return +10;
				return +30;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return -3;
				return -1;
            case Enemy.VEGAN:
                // return +3;
				return +2;
            default: return 0;
        }
    }
}
