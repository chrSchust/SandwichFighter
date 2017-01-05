
using System;

public class Tomato : Ingredient
{
    public string getName()
    {
        return "Tomaten";
    }

	public string getType() 
	{ 
		return IngredientType.VEGETABLE; 
	}

    public int getDamageBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return +5;
				return +10;
            case Enemy.VEGAN:
                // return -5;
				return 0;
            default: return 0;
        }
    }

    public int getSpeedBonus(int enemyType)
    {
        switch (enemyType)
        {
            case Enemy.FAT:
                // return +2;
				return +1;
            case Enemy.VEGAN:
                // return -2;
				return -1;
            default: return 0;
        }
    }
}
